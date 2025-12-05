using System.Threading;
using System.Threading.Tasks;

namespace TechSolutions.Core.Payments
{
    // Adapter para PayPal
    public class PayPalAdapter : IPaymentProcessor
    {
        private readonly PayPalService _payPalService;

        public PayPalAdapter(PayPalService payPalService)
        {
            _payPalService = payPalService;
        }

        public PaymentMethod Method => PaymentMethod.PayPal;

        public async Task<PaymentResult> ProcessAsync(
            PaymentRequest request,
            CancellationToken cancellationToken = default)
        {
            var transactionId = await _payPalService.PayAsync(
                request.CustomerIdentifier,
                request.Amount,
                request.Currency);

            return new PaymentResult
            {
                Success = true,
                TransactionId = transactionId
            };
        }
    }

    // Adapter para Yape
    public class YapeAdapter : IPaymentProcessor
    {
        private readonly YapeService _yapeService;

        public YapeAdapter(YapeService yapeService)
        {
            _yapeService = yapeService;
        }

        public PaymentMethod Method => PaymentMethod.Yape;

        public async Task<PaymentResult> ProcessAsync(
            PaymentRequest request,
            CancellationToken cancellationToken = default)
        {
            var code = await _yapeService.PagarAsync(
                request.CustomerIdentifier,
                request.Amount);

            return new PaymentResult
            {
                Success = true,
                TransactionId = $"YAPE-{code}"
            };
        }
    }

    // Adapter para Plin
    public class PlinAdapter : IPaymentProcessor
    {
        private readonly PlinService _plinService;

        public PlinAdapter(PlinService plinService)
        {
            _plinService = plinService;
        }

        public PaymentMethod Method => PaymentMethod.Plin;

        public async Task<PaymentResult> ProcessAsync(
            PaymentRequest request,
            CancellationToken cancellationToken = default)
        {
            var transactionId = await _plinService.ProcesarPagoAsync(
                request.CustomerIdentifier,
                request.Amount);

            return new PaymentResult
            {
                Success = true,
                TransactionId = transactionId
            };
        }
    }
}
