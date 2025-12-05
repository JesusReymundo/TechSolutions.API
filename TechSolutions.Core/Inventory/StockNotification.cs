namespace TechSolutions.Core.Inventory
{
    public class StockNotification
    {
        public string RecipientRole { get; set; } = string.Empty; // Manager, Purchases, etc.
        public string Message { get; set; } = string.Empty;
    }
}
