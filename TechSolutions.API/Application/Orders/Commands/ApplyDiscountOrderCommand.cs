using System;
using TechSolutions.API.Domain.Orders;

namespace TechSolutions.API.Application.Orders.Commands
{
    public class ApplyDiscountOrderCommand : IOrderCommand
    {
        private readonly IOrderRepository _repository;
        private readonly Guid _orderId;
        private readonly decimal _percentage;

        private Order? _beforeState;

        public ApplyDiscountOrderCommand(IOrderRepository repository, Guid orderId, decimal percentage)
        {
            _repository = repository;
            _orderId = orderId;
            _percentage = percentage;
        }

        public Order Execute()
        {
            var order = _repository.GetById(_orderId)
                       ?? throw new InvalidOperationException("Pedido no encontrado.");

            _beforeState = order.Clone();

            order.ApplyDiscount(_percentage);
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
