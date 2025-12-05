using TechSolutions.Core.Payments;

namespace TechSolutions.API.Dtos
{
    public class UpdatePaymentGatewayRequest
    {
        public PaymentMethod Method { get; set; }
    }
}
