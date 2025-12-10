using System;
using TechSolutions.API.Domain.Orders;

namespace TechSolutions.API.Application.Orders.Commands
{
    public class ProcessOrderCommand : IOrderCommand
    {
        private readonly IOrderRepository _repository;
        private readonly Guid _orderId;

        private Order? _beforeState;

        public ProcessOrderCommand(IOrderRepository repository, Guid orderId)
        {
            _repository = repository;
            _orderId = orderId;
        }

        public Order Execute()
        {
            var order = _repository.GetById(_orderId)
                       ?? throw new InvalidOperationException("Pedido no encontrado.");

            _beforeState = order.Clone();

            order.Process();
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
