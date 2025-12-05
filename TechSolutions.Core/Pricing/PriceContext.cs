namespace TechSolutions.Core.Pricing
{
    public class PriceContext
    {
        public decimal? DiscountPercentage { get; set; }
        public decimal? DemandFactor { get; set; }
        public bool IsPromotionActive { get; set; }
    }
}
