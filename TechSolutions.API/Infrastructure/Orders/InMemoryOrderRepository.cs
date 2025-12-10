using System;
using System.Collections.Generic;
using System.Linq;
using TechSolutions.API.Application.Orders;
using TechSolutions.API.Domain.Orders;

namespace TechSolutions.API.Infrastructure.Orders
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new();

        public IEnumerable<Order> GetAll() => _orders;

        public Order? GetById(Guid id) => _orders.FirstOrDefault(o => o.Id == id);

        public void Add(Order order)
        {
            _orders.Add(order);
        }

        public void Update(Order order)
        {
            var existing = GetById(order.Id);
            if (existing is null) return;

            _orders.Remove(existing);
            _orders.Add(order);
        }

        public void Delete(Guid id)
        {
            var existing = GetById(id);
            if (existing is not null)
            {
                _orders.Remove(existing);
            }
        }
    }
}
