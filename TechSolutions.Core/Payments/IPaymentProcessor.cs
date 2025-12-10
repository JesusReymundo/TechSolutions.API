namespace TechSolutions.Core.Payments
{
    public interface IPaymentProcessor
    {
        PaymentMethod Method { get; }

        PaymentResult ProcessPayment(PaymentRequest request);
    }
}
