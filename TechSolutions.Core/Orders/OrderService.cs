using System.Collections.Generic;

namespace TechSolutions.Core.Orders
{
    public class OrderService
    {
        private readonly Dictionary<int, Order> _orders = new();
        private readonly OrderHistory _history = new();
        private int _nextId = 1;

        public IReadOnlyCollection<Order> GetAll() => _orders.Values;

        public Order GetById(int id)
        {
            if (!_orders.TryGetValue(id, out var order))
                throw new KeyNotFoundException($"Pedido {id} no encontrado.");

            return order;
        }

        public Order CreateOrder(string customerName, decimal amount)
        {
            var order = new Order
            {
                Id = _nextId++,
                CustomerName = customerName,
                Amount = amount,
                DiscountPercentage = 0m,
                Status = OrderStatus.Created
            };

            _orders.Add(order.Id, order);
            _history.SaveState(order);

            return order;
        }

        public Order Process(int id)
        {
            var order = GetById(id);
            var command = new ProcessOrderCommand();
            command.Execute(order, _history);
            return order;
        }

        public Order ApplyDiscount(int id, decimal discount)
        {
            var order = GetById(id);
            var command = new ApplyDiscountCommand(discount);
            command.Execute(order, _history);
            return order;
        }

        public Order Cancel(int id)
        {
            var order = GetById(id);
            var command = new CancelOrderCommand();
            command.Execute(order, _history);
            return order;
        }

        public bool UndoLast(int id)
        {
            var order = GetById(id);
            return _history.TryRestoreLastState(order);
        }
    }
}
