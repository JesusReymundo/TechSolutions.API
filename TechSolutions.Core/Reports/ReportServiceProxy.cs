using System.Threading;
using System.Threading.Tasks;
using TechSolutions.Core.Security;

namespace TechSolutions.Core.Reports
{
    // Proxy que controla acceso según el rol del usuario
    public class ReportServiceProxy : IReportService
    {
        private readonly RealReportService _inner;
        private readonly ICurrentUserContext _currentUserContext;

        public ReportServiceProxy(
            RealReportService inner,
            ICurrentUserContext currentUserContext)
        {
            _inner = inner;
            _currentUserContext = currentUserContext;
        }

        public async Task<FinancialReport> GetMonthlyReportAsync(
            int year,
            int month,
            CancellationToken cancellationToken = default)
        {
            var user = _currentUserContext.GetCurrentUser();

            if (user.Role != UserRole.Manager &&
                user.Role != UserRole.Accountant)
            {
                throw new AuthorizationException(
                    $"El usuario '{user.UserName}' con rol '{user.Role}' no está autorizado para ver reportes financieros.");
            }

            var report = await _inner.GetMonthlyReportAsync(year, month, cancellationToken);
            report.GeneratedBy = user.UserName;
            return report;
        }
    }
}
