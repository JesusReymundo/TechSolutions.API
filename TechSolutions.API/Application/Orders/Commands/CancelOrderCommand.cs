using System;
using TechSolutions.API.Domain.Orders;

namespace TechSolutions.API.Application.Orders.Commands
{
    public class CancelOrderCommand : IOrderCommand
    {
        private readonly IOrderRepository _repository;
        private readonly Guid _orderId;

        private Order? _beforeState;

        public CancelOrderCommand(IOrderRepository repository, Guid orderId)
        {
            _repository = repository;
            _orderId = orderId;
        }

        public Order Execute()
        {
            var order = _repository.GetById(_orderId)
                       ?? throw new InvalidOperationException("Pedido no encontrado.");

            _beforeState = order.Clone();

            order.Cancel();
            _repository.Update(order);

            return order;
        }

        public void Undo()
        {
            if (_beforeState is null) return;

            var current = _repository.GetById(_beforeState.Id);
            if (current is null) return;

            current.RestoreFrom(_beforeState);
            _repository.Update(current);
        }
    }
}
