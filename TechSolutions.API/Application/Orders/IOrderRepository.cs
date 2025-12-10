using System;
using System.Collections.Generic;
using TechSolutions.API.Domain.Orders;

namespace TechSolutions.API.Application.Orders
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order? GetById(Guid id);
        void Add(Order order);
        void Update(Order order);
        void Delete(Guid id);
    }
}
