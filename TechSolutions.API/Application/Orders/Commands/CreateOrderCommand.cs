using TechSolutions.API.Domain.Orders;

namespace TechSolutions.API.Application.Orders.Commands
{
    public class CreateOrderCommand : IOrderCommand
    {
        private readonly IOrderRepository _repository;

        private readonly string _customerName;
        private readonly decimal _amount;
        private readonly string _description;

        private Order? _createdOrder;

        public CreateOrderCommand(
            IOrderRepository repository,
            string customerName,
            decimal amount,
            string description)
        {
            _repository = repository;
            _customerName = customerName;
            _amount = amount;
            _description = description;
        }

        public Order Execute()
        {
            var order = new Order(_customerName, _amount, _description);
            _repository.Add(order);
            _createdOrder = order;
            return order;
        }

        public void Undo()
        {
            if (_createdOrder is null) return;
            _repository.Delete(_createdOrder.Id);
        }
    }
}
