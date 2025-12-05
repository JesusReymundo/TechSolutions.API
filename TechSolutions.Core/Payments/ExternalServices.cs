using System;
using System.Threading.Tasks;

namespace TechSolutions.Core.Payments
{
    // Simula SDK de PayPal
    public class PayPalService
    {
        public Task<string> PayAsync(string email, decimal amount, string currency)
        {
            var transactionId = $"PAYPAL-{Guid.NewGuid()}";
            return Task.FromResult(transactionId);
        }
    }

    // Simula SDK de Yape
    public class YapeService
    {
        public Task<int> PagarAsync(string phoneNumber, decimal monto)
        {
            var code = Random.Shared.Next(100000, 999999);
            return Task.FromResult(code);
        }
    }

    // Simula SDK de Plin
    public class PlinService
    {
        public Task<string> ProcesarPagoAsync(string phoneNumber, decimal amount)
        {
            var transactionId = $"PLIN-{Guid.NewGuid()}";
            return Task.FromResult(transactionId);
        }
    }
}
