using System;
using System.Collections.Generic;
using System.Linq;
using TechSolutions.Core.Entities;

namespace TechSolutions.Core.Catalog
{
    internal class ProductIterator : IProductIterator
    {
        private readonly List<Product> _pageItems;
        private int _currentIndex = -1;

        public ProductIterator(
            ProductCatalog catalog,
            int pageNumber,
            int pageSize,
            string? nameFilter)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber));
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            IEnumerable<Product> query = catalog.Products;

            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                query = query.Where(p =>
                    p.Name.Contains(nameFilter, StringComparison.OrdinalIgnoreCase));
            }

            _pageItems = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public bool HasNext() => _currentIndex + 1 < _pageItems.Count;

        public Product Next()
        {
            if (!HasNext())
                throw new InvalidOperationException("No hay más elementos en esta página.");

            _currentIndex++;
            return _pageItems[_currentIndex];
        }
    }
}
