using TechSolutions.API.Domain.Orders;

namespace TechSolutions.API.Application.Orders.Commands
{
    public interface IOrderCommand
    {
        Order Execute();
        void Undo();
    }
}
