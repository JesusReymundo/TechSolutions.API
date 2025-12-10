using System;

namespace TechSolutions.Core.Orders
{
    public class Order
    {
        public Guid Id { get; private set; }
        public string CustomerName { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Amount { get; private set; }
        public decimal DiscountPercentage { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public decimal FinalAmount => Amount * (1 - DiscountPercentage / 100m);

        // ctor privado para serializadores
        private Order()
        {
        }

        public Order(string customerName, decimal amount, string description)
        {
            Id = Guid.NewGuid();
            CustomerName = customerName;
            Description = description;
            Amount = amount;
            DiscountPercentage = 0;
            Status = OrderStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void ApplyDiscount(decimal percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException(nameof(percentage),
                    "El descuento debe estar entre 0 y 100.");

            DiscountPercentage = percentage;
        }

        public void MarkProcessed()
        {
            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("No se puede procesar una orden cancelada.");

            Status = OrderStatus.Processed;
        }

        public void Cancel()
        {
            Status = OrderStatus.Cancelled;
        }

        // ----- Memento -----

        public OrderMemento CreateMemento()
        {
            return new OrderMemento(Status, Amount, DiscountPercentage);
        }

        public void Restore(OrderMemento memento)
        {
            if (memento == null) throw new ArgumentNullException(nameof(memento));

            Status = memento.Status;
            Amount = memento.Amount;
            DiscountPercentage = memento.DiscountPercentage;
        }
    }
}
