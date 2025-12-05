using TechSolutions.Core.Pricing;

namespace TechSolutions.API.Dtos
{
    public class UpdatePricingConfigRequest
    {
        public PriceStrategyType Strategy { get; set; }
        public decimal? DefaultDiscountPercentage { get; set; }
        public decimal? DefaultDemandFactor { get; set; }
    }
}
