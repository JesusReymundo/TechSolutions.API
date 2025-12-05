using TechSolutions.Core.Entities;

namespace TechSolutions.Core.Pricing
{
    public interface IPriceStrategy
    {
        PriceStrategyType StrategyType { get; }

        decimal CalculatePrice(Product product, PriceContext context);
    }
}
