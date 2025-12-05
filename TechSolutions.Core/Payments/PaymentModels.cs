namespace TechSolutions.Core.Payments
{
    public enum PaymentMethod
    {
        PayPal = 0,
        Yape = 1,
        Plin = 2
    }

    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "PEN";
        public PaymentMethod Method { get; set; }
        /// <summary>
        /// Email (PayPal) o número de celular (Yape/Plin)
        /// </summary>
        public string CustomerIdentifier { get; set; } = string.Empty;
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string? TransactionId { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
