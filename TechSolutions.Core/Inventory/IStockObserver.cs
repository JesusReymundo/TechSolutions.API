using System.Threading;
using System.Threading.Tasks;

namespace TechSolutions.Core.Inventory
{
    public interface IStockObserver
    {
        Task<StockNotification> OnStockBelowMinimumAsync(
            InventoryItem item,
            CancellationToken cancellationToken = default);
    }
}
