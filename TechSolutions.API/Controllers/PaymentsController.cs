using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TechSolutions.Core.Payments;

namespace TechSolutions.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentConfiguration _config;
        private readonly IPaymentProcessor[] _processors;

        public PaymentsController(PaymentConfiguration config, IEnumerable<IPaymentProcessor> processors)
        {
            _config = config;
            _processors = processors.ToArray();
        }

        [HttpPost]
        public ActionResult<PaymentResult> Process([FromBody] PaymentRequest request)
        {
            if (!_config.Enabled)
            {
                return BadRequest(new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "Los pagos están deshabilitados desde la configuración."
                });
            }

            if (!_config.EnabledMethods.Contains(request.Method))
            {
                return BadRequest(new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "El método de pago seleccionado no está habilitado."
                });
            }

            var processor = _processors.FirstOrDefault(p => p.Method == request.Method);
            if (processor == null)
            {
                return BadRequest(new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "No hay un adaptador configurado para este método."
                });
            }

            var result = processor.ProcessPayment(request);
            return Ok(result);
        }

        [HttpGet("config")]
        public ActionResult<object> GetConfig()
        {
            return Ok(new
            {
                enabled = _config.Enabled,
                enabledMethods = _config.EnabledMethods
            });
        }

        [HttpPost("config/enable")]
        public IActionResult Enable()
        {
            _config.Enabled = true;
            return Ok(new { enabled = true });
        }

        [HttpPost("config/disable")]
        public IActionResult Disable()
        {
            _config.Enabled = false;
            return Ok(new { enabled = false });
        }
    }
}
