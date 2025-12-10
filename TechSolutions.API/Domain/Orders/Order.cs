using System;

namespace TechSolutions.API.Domain.Orders
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CustomerName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
        public DateTime? CancelledAt { get; set; }

        public decimal DiscountPercentage { get; set; }
        public decimal FinalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Order()
        {
        }

        public Order(string customerName, decimal amount, string description)
        {
            Id = Guid.NewGuid();
            CustomerName = customerName;
            Amount = amount;
            Description = description;
            CreatedAt = DateTime.UtcNow;
            FinalAmount = amount;
            Status = OrderStatus.Pending;
        }

        public void Process()
        {
            if (Status != OrderStatus.Pending) return;
            Status = OrderStatus.Processed;
            ProcessedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Cancelled) return;
            Status = OrderStatus.Cancelled;
            CancelledAt = DateTime.UtcNow;
        }

        public void ApplyDiscount(decimal percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException(nameof(percentage));

            DiscountPercentage = percentage;
            FinalAmount = Math.Round(Amount * (1 - (percentage / 100m)), 2);
        }

        public void ClearDiscount()
        {
            DiscountPercentage = 0;
            FinalAmount = Amount;
        }

        public Order Clone()
        {
            return new Order
            {
                Id = this.Id,
                CustomerName = this.CustomerName,
                Amount = this.Amount,
                Description = this.Description,
                CreatedAt = this.CreatedAt,
                ProcessedAt = this.ProcessedAt,
                CancelledAt = this.CancelledAt,
                DiscountPercentage = this.DiscountPercentage,
                FinalAmount = this.FinalAmount,
                Status = this.Status
            };
        }

        public void RestoreFrom(Order source)
        {
            CustomerName = source.CustomerName;
            Amount = source.Amount;
            Description = source.Description;
            CreatedAt = source.CreatedAt;
            ProcessedAt = source.ProcessedAt;
            CancelledAt = source.CancelledAt;
            DiscountPercentage = source.DiscountPercentage;
            FinalAmount = source.FinalAmount;
            Status = source.Status;
        }
    }
}
