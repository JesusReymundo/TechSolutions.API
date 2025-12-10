namespace TechSolutions.API.Domain.Orders;

public class OrderMemento
{
    public Guid Id { get; }
    public string CustomerName { get; }
    public decimal Amount { get; }
    public OrderStatus Status { get; }
    public DateTime CreatedAt { get; }
    public DateTime? ProcessedAt { get; }
    public DateTime? CancelledAt { get; }

    public OrderMemento(
        Guid id,
        string customerName,
        decimal amount,
        OrderStatus status,
        DateTime createdAt,
        DateTime? processedAt,
        DateTime? cancelledAt)
    {
        Id = id;
        CustomerName = customerName;
        Amount = amount;
        Status = status;
        CreatedAt = createdAt;
        ProcessedAt = processedAt;
        CancelledAt = cancelledAt;
    }
}
