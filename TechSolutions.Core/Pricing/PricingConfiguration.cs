namespace TechSolutions.Core.Pricing
{
    public class PricingConfiguration
    {
        public PriceStrategyType DefaultStrategy { get; private set; } = PriceStrategyType.Standard;

        public decimal? DefaultDiscountPercentage { get; private set; }

        public decimal? DefaultDemandFactor { get; private set; }

        public void Update(
            PriceStrategyType strategy,
            decimal? discountPercentage,
            decimal? demandFactor)
        {
            DefaultStrategy = strategy;
            DefaultDiscountPercentage = discountPercentage;
            DefaultDemandFactor = demandFactor;
        }
    }
}
