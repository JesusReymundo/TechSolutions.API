using System;

namespace TechSolutions.Core.Payments
{
    public enum PaymentMethod
    {
        PayPal = 1,
        Yape = 2,
        Plin = 3
    }

    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "PEN";
        public PaymentMethod Method { get; set; }
        public string CustomerIdentifier { get; set; } = string.Empty;
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string? TransactionId { get; set; }
        public string? ErrorMessage { get; set; }

        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "PEN";

        public string ExecutedBy { get; set; } = string.Empty;
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    }
}
