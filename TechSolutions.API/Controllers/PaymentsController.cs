using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TechSolutions.API.Dtos;
using TechSolutions.Core.Payments;

namespace TechSolutions.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IEnumerable<IPaymentProcessor> _processors;
        private readonly PaymentConfiguration _configuration;

        public PaymentsController(
            IEnumerable<IPaymentProcessor> processors,
            PaymentConfiguration configuration)
        {
            _processors = processors;
            _configuration = configuration;
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<ActionResult<PaymentResult>> ProcessPayment(
            [FromBody] PaymentRequest request,
            CancellationToken cancellationToken)
        {
            if (!_configuration.IsEnabled(request.Method))
            {
                return BadRequest(new
                {
                    message = $"La pasarela {request.Method} se encuentra deshabilitada por configuración."
                });
            }

            var processor = _processors.FirstOrDefault(p => p.Method == request.Method);

            if (processor == null)
            {
                return BadRequest(new { message = "No existe un procesador para el método indicado." });
            }

            var result = await processor.ProcessAsync(request, cancellationToken);

            return Ok(result);
        }

        // ---------- RF2: configuración de pasarelas ----------

        // GET: api/Payments/config
        [HttpGet("config")]
        public ActionResult GetConfig()
        {
            var enabled = _configuration.GetEnabledMethods()
                .Select(m => m.ToString())
                .ToList();

            return Ok(new { enabledMethods = enabled });
        }

        // POST: api/Payments/config/enable
        [HttpPost("config/enable")]
        public ActionResult Enable([FromBody] UpdatePaymentGatewayRequest request)
        {
            _configuration.Enable(request.Method);
            return NoContent();
        }

        // POST: api/Payments/config/disable
        [HttpPost("config/disable")]
        public ActionResult Disable([FromBody] UpdatePaymentGatewayRequest request)
        {
            _configuration.Disable(request.Method);
            return NoContent();
        }
    }
}
