using System;
using TechSolutions.Core.Entities;

namespace TechSolutions.Core.Pricing
{
    public class StandardPriceStrategy : IPriceStrategy
    {
        public PriceStrategyType StrategyType => PriceStrategyType.Standard;

        public decimal CalculatePrice(Product product, PriceContext context)
        {
            return product.BasePrice;
        }
    }

    public class DiscountPriceStrategy : IPriceStrategy
    {
        private const decimal DefaultDiscount = 0.10m; // 10%

        public PriceStrategyType StrategyType => PriceStrategyType.Discount;

        public decimal CalculatePrice(Product product, PriceContext context)
        {
            var discount = context.DiscountPercentage ?? DefaultDiscount;

            if (discount < 0m || discount > 0.90m)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(context.DiscountPercentage),
                    "El descuento debe estar entre 0% y 90%.");
            }

            var finalPrice = product.BasePrice * (1 - discount);
            return decimal.Round(finalPrice, 2);
        }
    }

    public class DynamicPriceStrategy : IPriceStrategy
    {
        private const decimal DefaultDemandFactor = 1.0m;

        public PriceStrategyType StrategyType => PriceStrategyType.Dynamic;

        public decimal CalculatePrice(Product product, PriceContext context)
        {
            var factor = context.DemandFactor ?? DefaultDemandFactor;

            if (factor < 0.5m || factor > 2.0m)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(context.DemandFactor),
                    "El factor de demanda debe estar entre 0.5 y 2.0.");
            }

            var finalPrice = product.BasePrice * factor;
            return decimal.Round(finalPrice, 2);
        }
    }
}
