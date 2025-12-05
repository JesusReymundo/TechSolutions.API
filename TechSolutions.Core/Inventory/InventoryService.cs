using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TechSolutions.Core.Inventory
{
    public class InventoryService
    {
        private readonly List<InventoryItem> _items;
        private readonly IEnumerable<IStockObserver> _observers;

        public InventoryService(IEnumerable<IStockObserver> observers)
        {
            _observers = observers;

            _items = new List<InventoryItem>
            {
                new InventoryItem { ProductId = 1, ProductName = "Laptop básica",  Stock = 20, MinimumStock = 5 },
                new InventoryItem { ProductId = 2, ProductName = "Impresora láser", Stock = 8,  MinimumStock = 3 },
                new InventoryItem { ProductId = 3, ProductName = "Router WiFi",    Stock = 15, MinimumStock = 4 }
            };
        }

        public Task<IReadOnlyList<InventoryItem>> GetAllAsync()
        {
            return Task.FromResult((IReadOnlyList<InventoryItem>)_items);
        }

        public async Task<(InventoryItem item, List<StockNotification> notifications)> AdjustStockAsync(
            int productId,
            int delta,
            CancellationToken cancellationToken = default)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
                throw new KeyNotFoundException($"Producto con id {productId} no encontrado.");

            var previousStock = item.Stock;
            item.Stock += delta;

            var notifications = new List<StockNotification>();

            if (item.Stock < item.MinimumStock && previousStock >= item.MinimumStock)
            {
                foreach (var observer in _observers)
                {
                    var notification = await observer.OnStockBelowMinimumAsync(item, cancellationToken);
                    notifications.Add(notification);
                }
            }

            return (item, notifications);
        }

        // ---------- RF6: configurar stock mínimo por producto ----------
        public Task<InventoryItem> UpdateMinimumStockAsync(
            int productId,
            int minimumStock,
            CancellationToken cancellationToken = default)
        {
            if (minimumStock < 0)
                throw new ArgumentOutOfRangeException(nameof(minimumStock),
                    "El stock mínimo no puede ser negativo.");

            var item = _items.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
                throw new KeyNotFoundException($"Producto con id {productId} no encontrado.");

            item.MinimumStock = minimumStock;
            return Task.FromResult(item);
        }
    }
}
