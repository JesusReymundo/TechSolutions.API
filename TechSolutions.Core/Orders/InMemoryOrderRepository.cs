using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TechSolutions.Core.Orders
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly ConcurrentDictionary<Guid, Order> _orders = new();

        public void Add(Order order)
        {
            _orders[order.Id] = order;
        }

        public void Update(Order order)
        {
            _orders[order.Id] = order;
        }

        public Order? GetById(Guid id)
        {
            _orders.TryGetValue(id, out var order);
            return order;
        }

        public IEnumerable<Order> GetAll()
        {
            return _orders.Values;
        }
    }
}
