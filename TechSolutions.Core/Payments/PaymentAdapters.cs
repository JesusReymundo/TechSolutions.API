using System;

namespace TechSolutions.Core.Payments
{
    public class PayPalAdapter : IPaymentProcessor
    {
        public PaymentMethod Method => PaymentMethod.PayPal;

        public PaymentResult ProcessPayment(PaymentRequest request)
        {
            return new PaymentResult
            {
                Success = true,
                TransactionId = $"PAYPAL-{Guid.NewGuid():N}".Substring(0, 16),
                Method = Method,
                Amount = request.Amount,
                Currency = request.Currency,
                ExecutedBy = "PayPalAdapter",
                ExecutedAt = DateTime.UtcNow
            };
        }
    }

    public class YapeAdapter : IPaymentProcessor
    {
        public PaymentMethod Method => PaymentMethod.Yape;

        public PaymentResult ProcessPayment(PaymentRequest request)
        {
            return new PaymentResult
            {
                Success = true,
                TransactionId = $"YAPE-{Guid.NewGuid():N}".Substring(0, 16),
                Method = Method,
                Amount = request.Amount,
                Currency = request.Currency,
                ExecutedBy = "YapeAdapter",
                ExecutedAt = DateTime.UtcNow
            };
        }
    }

    public class PlinAdapter : IPaymentProcessor
    {
        public PaymentMethod Method => PaymentMethod.Plin;

        public PaymentResult ProcessPayment(PaymentRequest request)
        {
            return new PaymentResult
            {
                Success = true,
                TransactionId = $"PLIN-{Guid.NewGuid():N}".Substring(0, 16),
                Method = Method,
                Amount = request.Amount,
                Currency = request.Currency,
                ExecutedBy = "PlinAdapter",
                ExecutedAt = DateTime.UtcNow
            };
        }
    }
}
