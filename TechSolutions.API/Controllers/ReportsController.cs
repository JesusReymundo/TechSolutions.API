using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechSolutions.Core.Reports;
using TechSolutions.Core.Security;

namespace TechSolutions.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET: api/Reports/monthly?year=2025&month=12&userName=Ana&role=Manager
        [HttpGet("monthly")]
        public async Task<ActionResult<FinancialReport>> GetMonthly(
            [FromQuery] int year,
            [FromQuery] int month,
            CancellationToken cancellationToken)
        {
            try
            {
                var report = await _reportService.GetMonthlyReportAsync(
                    year,
                    month,
                    cancellationToken);

                return Ok(report);
            }
            catch (AuthorizationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
        }
    }
}
