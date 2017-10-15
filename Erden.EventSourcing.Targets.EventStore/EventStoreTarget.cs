using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

using EventStore.ClientAPI;
using EventStore.ClientAPI.Exceptions;
using EventStore.ClientAPI.SystemData;
using Newtonsoft.Json;

namespace Erden.EventSourcing.Targets.EventStore
{
    /// <summary>
    /// EventStore target for <see cref="IEventStore"/>
    /// </summary>
    public class EventStoreTarget : IEventStore, IDisposable
    {
        /// <summary>
        /// Constant for pagination while reading all events in stream
        /// </summary>
        private const int COUNT = 500;
        private readonly ConnectionSettingsBuilder builder;
        private readonly IPEndPoint eventStoreEndPoint;
        private readonly UserCredentials credentials;
        private readonly IEventPublisher publisher;
        private readonly IEventStoreConnection connection;

        public EventStoreTarget(IOptions<EventStoreSettings> settings, IEventPublisher publisher)
        {
            if (!IPAddress.TryParse(settings.Value.Address, out var address))
                throw new Exception();

            this.publisher = publisher;

            eventStoreEndPoint = new IPEndPoint(address, settings.Value.Port);
            credentials = new UserCredentials(settings.Value.Username, settings.Value.Password);
            builder = ConnectionSettings.Create();
            connection = EventStoreConnection.Create(builder, eventStoreEndPoint);
            connection.ConnectAsync().Wait();
        }

        public async Task Add(IEvent @event, long version)
        {
            try
            {
                await connection.AppendToStreamAsync(
                    @event.EntityId.ToString(),
                    version,
                    Serialize(@event));

                publisher.Publish(@event);
            }
            catch (WrongExpectedVersionException e)
            {
                throw;
            }
        }

        public async Task Add(IReadOnlyCollection<IEvent> events, long version)
        {
            var id = events.ElementAt(0).EntityId;
            try
            {
                await connection.AppendToStreamAsync(
                    id.ToString(),
                    version,
                    events.Select(@event => Serialize(@event)));

                await Task.Run(() =>
                {
                    foreach (var @event in events)
                        publisher.Publish(@event);
                });
            }
            catch (WrongExpectedVersionException e)
            {
                throw;
            }
        }

        public async Task<IReadOnlyCollection<IEvent>> Get(Guid id)
        {
            return await ReadAllEvents(connection, id.ToString(), 0, new List<IEvent>());
        }

        /// <summary>
        /// Recursively reads all events from stream
        /// </summary>
        /// <param name="connection">Eventstore connection</param>
        /// <param name="stream">Stream ID</param>
        /// <param name="start">Start position</param>
        /// <param name="events">Container for read events</param>
        /// <returns>All events from stream</returns>
        private async Task<IReadOnlyCollection<IEvent>> ReadAllEvents(IEventStoreConnection connection, string stream, long start, List<IEvent> events)
        {
            var slice = await connection.ReadStreamEventsForwardAsync(stream, start, COUNT, false, credentials);
            events.AddRange(slice.Events.Select(e => Deserialize(e.Event)).ToList());
            if (slice.IsEndOfStream)
                return events;
            return await ReadAllEvents(connection, stream, start + COUNT, events);
        }

        /// <summary>
        /// Convert <see cref="IEvent"/> to <see cref="EventData"/>
        /// </summary>
        /// <param name="event">Event to convert</param>
        /// <returns>Event prepared to write</returns>
        private EventData Serialize(IEvent @event)
        {
            var @string = JsonConvert.SerializeObject(@event);
            var metadata = JsonConvert.SerializeObject(new EventMetadata
            {
                AssemblyQualifiedName = @event.GetType().AssemblyQualifiedName
            });
            return new EventData(
                @event.Id,
                @event.GetType().Name,
                true,
                Encoding.UTF8.GetBytes(@string),
                Encoding.UTF8.GetBytes(metadata));
        }

        /// <summary>
        /// Deserialization of <see cref="IEvent"/>
        /// </summary>
        /// <param name="event">Event from EventStore</param>
        /// <returns>Domain event</returns>
        private IEvent Deserialize(RecordedEvent @event)
        {
            string data = Encoding.UTF8.GetString(@event.Data);
            var metadata = JsonConvert.DeserializeObject<EventMetadata>(Encoding.UTF8.GetString(@event.Metadata));
            return JsonConvert.DeserializeObject(data, Type.GetType(metadata.AssemblyQualifiedName)) as IEvent;
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}