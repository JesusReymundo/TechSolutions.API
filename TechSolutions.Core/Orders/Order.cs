using System;

namespace TechSolutions.Core.Orders
{
    public class Order
    {
        public int Id { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        /// <summary>
        /// Descuento aplicado en rango [0,1]. Ej: 0.15 = 15%
        /// </summary>
        public decimal DiscountPercentage { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Created;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public decimal FinalAmount => Math.Round(Amount * (1 - DiscountPercentage), 2);
    }
}
