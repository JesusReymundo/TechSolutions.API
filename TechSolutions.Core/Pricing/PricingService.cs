using System;
using System.Collections.Generic;
using System.Linq;
using TechSolutions.Core.Entities;

namespace TechSolutions.Core.Pricing
{
    public class PricingService
    {
        private readonly IDictionary<int, Product> _products;
        private readonly IDictionary<PriceStrategyType, IPriceStrategy> _strategies;

        public PricingService(IEnumerable<IPriceStrategy> strategies)
        {
            _strategies = strategies.ToDictionary(s => s.StrategyType);

            // Catálogo simple en memoria (luego nos sirve para Iterator)
            _products = new Dictionary<int, Product>
            {
                { 1, new Product { Id = 1, Name = "Plan Básico ERP",        BasePrice = 100m } },
                { 2, new Product { Id = 2, Name = "Plan Pyme Plus",         BasePrice = 180m } },
                { 3, new Product { Id = 3, Name = "Plan Full Corporativo",  BasePrice = 320m } }
            };
        }

        public IReadOnlyCollection<Product> GetAllProducts()
            => _products.Values.ToList();

        public Product GetProduct(int productId)
        {
            if (!_products.TryGetValue(productId, out var product))
                throw new KeyNotFoundException($"Producto con id {productId} no encontrado.");

            return product;
        }

        public decimal CalculatePrice(
            int productId,
            PriceStrategyType strategyType,
            PriceContext context)
        {
            var product = GetProduct(productId);

            if (!_strategies.TryGetValue(strategyType, out var strategy))
            {
                throw new InvalidOperationException(
                    $"Estrategia de precios no soportada: {strategyType}");
            }

            return strategy.CalculatePrice(product, context);
        }
    }
}
