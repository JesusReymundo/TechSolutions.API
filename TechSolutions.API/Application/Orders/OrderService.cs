using System;
using System.Collections.Generic;
using TechSolutions.API.Application.Orders.Commands;
using TechSolutions.API.Domain.Orders;

namespace TechSolutions.API.Application.Orders
{
    public class OrderService
    {
        private readonly IOrderRepository _repository;
        private readonly Stack<IOrderCommand> _history = new();

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Order> GetAll() => _repository.GetAll();

        public Order? GetById(Guid id) => _repository.GetById(id);

        public Order CreateOrder(string customerName, decimal amount, string description)
        {
            var cmd = new CreateOrderCommand(_repository, customerName, amount, description);
            var order = cmd.Execute();
            _history.Push(cmd);
            return order;
        }

        public Order? ProcessOrder(Guid id)
        {
            if (_repository.GetById(id) is null) return null;

            var cmd = new ProcessOrderCommand(_repository, id);
            var order = cmd.Execute();
            _history.Push(cmd);
            return order;
        }

        public Order? CancelOrder(Guid id)
        {
            if (_repository.GetById(id) is null) return null;

            var cmd = new CancelOrderCommand(_repository, id);
            var order = cmd.Execute();
            _history.Push(cmd);
            return order;
        }

        public Order? ApplyDiscount(Guid id, decimal percentage)
        {
            if (_repository.GetById(id) is null) return null;

            var cmd = new ApplyDiscountOrderCommand(_repository, id, percentage);
            var order = cmd.Execute();
            _history.Push(cmd);
            return order;
        }

        public bool UndoLastCommand()
        {
            if (_history.Count == 0) return false;

            var last = _history.Pop();
            last.Undo();
            return true;
        }
    }
}
