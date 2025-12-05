namespace TechSolutions.API.Dtos
{
    public class UpdateMinimumStockRequest
    {
        public int ProductId { get; set; }
        public int MinimumStock { get; set; }
    }
}
