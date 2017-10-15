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
            ApplyChange(new OrderOpenedEvent(id, position, count));
        }

        public void Approve()
        {
            ApplyChange(new OrderApprovedEvent(Id, Version + 1));
        }

        public void Close()
        {
            ApplyChange(new OrderClosedEvent(Id, Version + 1));
        }

        public void Complete()
        {
            ApplyChange(new OrderCompletedEvent(Id, Version + 1));
        }

        public void Decline()
        {
            ApplyChange(new OrderDeclinedEvent(Id, Version + 1));
        }

        public void Reopen(int newCount)
        {
            ApplyChange(new OrderReopenedEvent(Id, newCount, Version + 1));
        }

        public void SetResponsible(Guid responsibleId)
        {
            ApplyChange(new OrderResponsibleSetEvent(Id, responsibleId, Version + 1));
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