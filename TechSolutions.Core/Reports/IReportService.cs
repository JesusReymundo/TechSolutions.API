using System.Threading;
using System.Threading.Tasks;

namespace TechSolutions.Core.Reports
{
    public interface IReportService
    {
        Task<FinancialReport> GetMonthlyReportAsync(
            int year,
            int month,
            CancellationToken cancellationToken = default);
    }
}
