using System.Collections.Generic;
using TechSolutions.Core.Entities;

namespace TechSolutions.Core.Catalog
{
    public class ProductCatalog : IProductCollection
    {
        private readonly List<Product> _products;

        public ProductCatalog()
        {
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Plan Básico ERP",       BasePrice = 100m },
                new Product { Id = 2, Name = "Plan Pyme Plus",        BasePrice = 180m },
                new Product { Id = 3, Name = "Plan Full Corporativo", BasePrice = 320m },
                new Product { Id = 4, Name = "Módulo Inventario",     BasePrice = 80m  },
                new Product { Id = 5, Name = "Módulo Facturación",    BasePrice = 120m },
                new Product { Id = 6, Name = "Módulo Reportes",       BasePrice = 90m  }
            };
        }

        internal IReadOnlyList<Product> Products => _products;

        public IProductIterator CreateIterator(
            int pageNumber,
            int pageSize,
            string? nameFilter = null)
        {
            return new ProductIterator(this, pageNumber, pageSize, nameFilter);
        }
    }
}
