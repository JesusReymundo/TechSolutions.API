using System;
using System.Collections.Generic;

namespace TechSolutions.Core.Orders
{
    /// <summary>
    /// Servicio de dominio para pedidos (Command + Memento).
    /// </summary>
    public class OrderService
    {
        private readonly IOrderRepository _repository;
        private readonly OrderCommandHistory _history;

        public OrderService(IOrderRepository repository, OrderCommandHistory history)
        {
            _repository = repository;
            _history = history;
        }

        // -------- Consultas --------

        public IEnumerable<Order> GetAllOrders()
            => _repository.GetAll();

        public Order? GetOrderById(Guid id)
            => _repository.GetById(id);

        // -------- Comandos --------

        public Order CreateOrder(string customerName, decimal amount, string description)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("Customer name is required.", nameof(customerName));
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            var order = new Order(customerName, amount, description);

            _repository.Add(order);

            IOrderCommand command = new CreateOrderCommand(customerName, amount, description);
            _history.SaveSnapshot(order, command);
            command.Execute(order);

            return order;
        }

        public Order? ProcessOrder(Guid id)
        {
            var order = _repository.GetById(id);
            if (order == null) return null;

            IOrderCommand command = new ProcessOrderCommand();
            _history.SaveSnapshot(order, command);
            command.Execute(order);
            _repository.Update(order);

            return order;
        }

        public Order? CancelOrder(Guid id)
        {
            var order = _repository.GetById(id);
            if (order == null) return null;

            IOrderCommand command = new CancelOrderCommand();
            _history.SaveSnapshot(order, command);
            command.Execute(order);
            _repository.Update(order);

            return order;
        }

        public Order? ApplyDiscount(Guid id, decimal percentage)
        {
            var order = _repository.GetById(id);
            if (order == null) return null;

            IOrderCommand command = new ApplyDiscountOrderCommand(percentage);
            _history.SaveSnapshot(order, command);
            command.Execute(order);
            _repository.Update(order);

            return order;
        }

        public Order? UndoLast(Guid id)
        {
            var order = _repository.GetById(id);
            if (order == null) return null;

            var restored = _history.TryRestoreLastSnapshot(order);
            if (!restored) return null;

            _repository.Update(order);
            return order;
        }
    }
}
