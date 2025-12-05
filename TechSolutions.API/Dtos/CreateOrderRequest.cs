namespace TechSolutions.API.Dtos
{
    public class CreateOrderRequest
    {
        public string CustomerName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
