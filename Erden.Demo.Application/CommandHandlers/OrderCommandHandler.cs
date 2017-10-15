using System;
using System.Threading.Tasks;

using Erden.Cqrs;
using Erden.Demo.Application.Aggregates;
using Erden.Demo.Application.Commands;
using Erden.Domain;

namespace Erden.Demo.Application.CommandHandlers
{
    public class OrderCommandHandler :
        ICommandHandler<OpenOrderCommand>,
        ICommandHandler<ApproveOrderCommand>,
        ICommandHandler<DeclineOrderCommand>,
        ICommandHandler<ReopenOrderCommand>,
        ICommandHandler<SetOrderResponsibleCommand>,
        ICommandHandler<CompleteOrderCommand>,
        ICommandHandler<CloseOrderCommand>
    {
        private readonly ISession session;

        public OrderCommandHandler(ISession session)
        {
            this.session = session;
        }

        public async Task Execute(OpenOrderCommand command)
        {
            OrderAggregate aggregate = new OrderAggregate(command.EntityId, command.Position, command.Count);
            await session.Add(aggregate);
            await session.Commit(command.EntityId);
        }

        public async Task Execute(ApproveOrderCommand command)
        {
            OrderAggregate aggregate = await Retrieve(command.EntityId);
            aggregate.Approve();
            await session.Commit(command.EntityId);
        }

        public async Task Execute(DeclineOrderCommand command)
        {
            OrderAggregate aggregate = await Retrieve(command.EntityId);
            aggregate.Decline();
            await session.Commit(command.EntityId);
        }

        public async Task Execute(ReopenOrderCommand command)
        {
            OrderAggregate aggregate = await Retrieve(command.EntityId);
            aggregate.Reopen(command.NewCount);
            await session.Commit(command.EntityId);
        }

        public async Task Execute(SetOrderResponsibleCommand command)
        {
            OrderAggregate aggregate = await Retrieve(command.EntityId);
            aggregate.SetResponsible(command.ResponsibleId);
            await session.Commit(command.EntityId);
        }

        public async Task Execute(CompleteOrderCommand command)
        {
            OrderAggregate aggregate = await Retrieve(command.EntityId);
            aggregate.Complete();
            await session.Commit(command.EntityId);
        }

        public async Task Execute(CloseOrderCommand command)
        {
            OrderAggregate aggregate = await Retrieve(command.EntityId);
            aggregate.Close();
            await session.Commit(command.EntityId);
        }

        private async Task<OrderAggregate> Retrieve(Guid id)
        {
            return await session.Get<OrderAggregate>(id, (int)EventSourcing.Version.Any);
        }
    }
}