namespace TechSolutions.Core.Orders
{
    public class OrderMemento
    {
        public OrderStatus Status { get; }
        public decimal Amount { get; }
        public decimal DiscountPercentage { get; }

        public OrderMemento(OrderStatus status, decimal amount, decimal discountPercentage)
        {
            Status = status;
            Amount = amount;
            DiscountPercentage = discountPercentage;
        }
    }
}
