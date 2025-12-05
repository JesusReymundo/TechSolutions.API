using System.Collections.Generic;
using TechSolutions.Core.Entities;

namespace TechSolutions.Core.Catalog
{
    public class CatalogService
    {
        private readonly IProductCollection _collection;

        public CatalogService(ProductCatalog catalog)
        {
            _collection = catalog;
        }

        public IReadOnlyCollection<Product> GetPage(
            int pageNumber,
            int pageSize,
            string? nameFilter = null)
        {
            var iterator = _collection.CreateIterator(pageNumber, pageSize, nameFilter);
            var result = new List<Product>();

            while (iterator.HasNext())
            {
                result.Add(iterator.Next());
            }

            return result;
        }
    }
}
