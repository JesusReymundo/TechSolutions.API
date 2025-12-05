using System.Threading;
using System.Threading.Tasks;

namespace TechSolutions.Core.Inventory
{
    // Observador para el rol Gerente
    public class ManagerStockObserver : IStockObserver
    {
        public Task<StockNotification> OnStockBelowMinimumAsync(
            InventoryItem item,
            CancellationToken cancellationToken = default)
        {
            var notification = new StockNotification
            {
                RecipientRole = "Manager",
                Message = $"ALERTA GERENCIA: El producto '{item.ProductName}' tiene stock bajo ({item.Stock}) por debajo del mínimo ({item.MinimumStock})."
            };

            return Task.FromResult(notification);
        }
    }

    // Observador para el rol Compras
    public class PurchasingStockObserver : IStockObserver
    {
        public Task<StockNotification> OnStockBelowMinimumAsync(
            InventoryItem item,
            CancellationToken cancellationToken = default)
        {
            var notification = new StockNotification
            {
                RecipientRole = "Purchases",
                Message = $"ALERTA COMPRAS: Reponer stock de '{item.ProductName}'. Actual: {item.Stock}, mínimo: {item.MinimumStock}."
            };

            return Task.FromResult(notification);
        }
    }
}
