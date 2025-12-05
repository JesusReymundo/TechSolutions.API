using System.Collections.Generic;
using TechSolutions.Core.Entities;
using TechSolutions.Core.Pricing;
using Xunit;

namespace TechSolutions.Tests
{
    public class PricingServiceTests
    {
        private PricingService CreateService()
        {
            var strategies = new List<IPriceStrategy>
            {
                new StandardPriceStrategy(),
                new DiscountPriceStrategy(),
                new DynamicPriceStrategy()
            };

            return new PricingService(strategies);
        }

        [Fact]
        public void StandardStrategy_Returns_BasePrice()
        {
            var service = CreateService();

            var price = service.CalculatePrice(
                productId: 1,
                strategyType: PriceStrategyType.Standard,
                context: new PriceContext());

            Assert.Equal(100m, price);
        }

        [Fact]
        public void DiscountStrategy_Applies_Discount()
        {
            var service = CreateService();

            var price = service.CalculatePrice(
                productId: 1,
                strategyType: PriceStrategyType.Discount,
                context: new PriceContext { DiscountPercentage = 0.20m });

            Assert.Equal(80m, price); // 100 - 20%
        }
    }
}
