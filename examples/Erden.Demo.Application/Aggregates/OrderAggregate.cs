using System;

using Erden.Demo.Application.Events;
using Erden.Domain;

namespace Erden.Demo.Application.Aggregates
{
    public enum OrderStatus
    {
        New = 0,
        Approving = 1,
        Approved = 2,
        InProgress = 3,
        Declined = -1,
        Closed = -2,
        Completed = 4
    }

    public class OrderAggregate : AggregateRoot
    {
        private string position;
        private int count;
        private OrderStatus status;
        private Guid responsibleId;

        public OrderAggregate() { }

        public OrderAggregate(Guid id, string position, int count)
        {
            ApplyChange<OrderOpenedEvent>(id, position, count);
        }

        public void Approve()
        {
            ApplyChange<OrderApprovedEvent>(Id);
        }

        public void Close()
        {
            ApplyChange<OrderClosedEvent>(Id);
        }

        public void Complete()
        {
            ApplyChange<OrderCompletedEvent>(Id);
        }

        public void Decline()
        {
            ApplyChange<OrderDeclinedEvent>(Id);
        }

        public void Reopen(int newCount)
        {
            ApplyChange<OrderReopenedEvent>(Id, newCount);
        }

        public void SetResponsible(Guid responsibleId)
        {
            ApplyChange<OrderResponsibleSetEvent>(Id, responsibleId);
        }

        private void Apply(OrderOpenedEvent @event)
        {
            Id = @event.EntityId;
            position = @event.Position;
            count = @event.Count;
            status = OrderStatus.New;
        }

        private void Apply(OrderApprovedEvent @event)
        {
            if (status != OrderStatus.New && status != OrderStatus.Approving)
                throw new Exception("Only new or declined orders could be approved");
            status = OrderStatus.Approved;
        }

        private void Apply(OrderDeclinedEvent @event)
        {
            if (status != OrderStatus.New && status != OrderStatus.Approved)
                throw new Exception("Only new or approved orders could be declined");
            status = OrderStatus.Declined;
        }

        private void Apply(OrderClosedEvent @event)
        {
            if (status != OrderStatus.Declined)
                throw new Exception("Only declined orders could be closed");
            status = OrderStatus.Closed;
        }

        private void Apply(OrderCompletedEvent @event)
        {
            if (status != OrderStatus.InProgress)
                throw new Exception("Only processing orders could be completed");
            status = OrderStatus.Completed;
        }

        private void Apply(OrderResponsibleSetEvent @event)
        {
            if (status != OrderStatus.Approved)
                throw new Exception("Only approved orders could be took in work");
            status = OrderStatus.InProgress;
            responsibleId = @event.ResponsibleId;
        }

        private void Apply(OrderReopenedEvent @event)
        {
            if (status != OrderStatus.Declined)
                throw new Exception("Only declined orders could be reopened");
            status = OrderStatus.Approving;
            count = @event.NewCount;
        }
    }
}