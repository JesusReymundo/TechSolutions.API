using System.Collections.Generic;

namespace TechSolutions.Core.Payments
{
    public class PaymentConfiguration
    {
        private readonly HashSet<PaymentMethod> _enabledMethods = new()
        {
            PaymentMethod.PayPal,
            PaymentMethod.Yape,
            PaymentMethod.Plin
        };

        public IReadOnlyCollection<PaymentMethod> GetEnabledMethods() => _enabledMethods;

        public bool IsEnabled(PaymentMethod method) => _enabledMethods.Contains(method);

        public void Enable(PaymentMethod method) => _enabledMethods.Add(method);

        public void Disable(PaymentMethod method) => _enabledMethods.Remove(method);
    }
}
