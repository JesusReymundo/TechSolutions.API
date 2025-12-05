using System.Threading;
using System.Threading.Tasks;

namespace TechSolutions.Core.Reports
{
    // "Sujeto real": calcula el reporte (simulado)
    public class RealReportService : IReportService
    {
        public Task<FinancialReport> GetMonthlyReportAsync(
            int year,
            int month,
            CancellationToken cancellationToken = default)
        {
            // Simulamos datos que vendrían de BD
            var totalSales = 100_000m + month * 1_000;
            var totalCosts = 60_000m + month * 800;

            var report = new FinancialReport
            {
                Year = year,
                Month = month,
                TotalSales = totalSales,
                TotalCosts = totalCosts,
                GeneratedAt = DateTime.UtcNow,
                GeneratedBy = "FinancialEngine"
            };

            return Task.FromResult(report);
        }
    }
}
