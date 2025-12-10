using System;
using System.Collections.Generic;

namespace TechSolutions.Core.Orders
{
    public interface IOrderRepository
    {
        void Add(Order order);
        void Update(Order order);
        Order? GetById(Guid id);
        IEnumerable<Order> GetAll();
    }
}
