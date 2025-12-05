using TechSolutions.Core.Pricing;

namespace TechSolutions.API.Dtos
{
    public class ApplyPriceRequest
    {
        public int ProductId { get; set; }

        /// <summary>
        /// Estrategia a utilizar. Si es null y UseConfiguredStrategy es true,
        /// se usa la estrategia configurada globalmente.
        /// </summary>
        public PriceStrategyType? Strategy { get; set; }

        public decimal? DiscountPercentage { get; set; }
        public decimal? DemandFactor { get; set; }

        /// <summary>
        /// Si es true, se usa la estrategia y parámetros configurados por el administrador.
        /// </summary>
        public bool UseConfiguredStrategy { get; set; } = false;
    }
}
