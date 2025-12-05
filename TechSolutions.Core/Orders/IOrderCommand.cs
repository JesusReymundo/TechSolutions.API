namespace TechSolutions.Core.Orders
{
    public interface IOrderCommand
    {
        string Name { get; }

        void Execute(Order order, OrderHistory history);
    }
}
