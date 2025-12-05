using System.Threading;
using System.Threading.Tasks;

namespace TechSolutions.Core.Payments
{
    /// <summary>
    /// Target del patrón Adapter
    /// </summary>
    public interface IPaymentProcessor
    {
        PaymentMethod Method { get; }

        Task<PaymentResult> ProcessAsync(
            PaymentRequest request,
            CancellationToken cancellationToken = default);
    }
}
