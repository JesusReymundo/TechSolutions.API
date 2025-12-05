namespace TechSolutions.Core.Inventory
{
    public class InventoryItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public int Stock { get; set; }
        public int MinimumStock { get; set; }
    }
}
