using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TechSolutions.API.Dtos;
using TechSolutions.Core.Entities;
using TechSolutions.Core.Pricing;

namespace TechSolutions.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricingController : ControllerBase
    {
        private readonly PricingService _pricingService;
        private readonly PricingConfiguration _configuration;

        public PricingController(
            PricingService pricingService,
            PricingConfiguration configuration)
        {
            _pricingService = pricingService;
            _configuration = configuration;
        }

        // GET: api/Pricing/products
        [HttpGet("products")]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _pricingService.GetAllProducts();
            return Ok(products);
        }

        // ---------- RF10: configuración de estrategia por defecto ----------

        // GET: api/Pricing/config
        [HttpGet("config")]
        public ActionResult GetConfig()
        {
            return Ok(new
            {
                defaultStrategy = _configuration.DefaultStrategy.ToString(),
                defaultDiscountPercentage = _configuration.DefaultDiscountPercentage,
                defaultDemandFactor = _configuration.DefaultDemandFactor
            });
        }

        // PUT: api/Pricing/config
        [HttpPut("config")]
        public ActionResult UpdateConfig([FromBody] UpdatePricingConfigRequest request)
        {
            _configuration.Update(
                request.Strategy,
                request.DefaultDiscountPercentage,
                request.DefaultDemandFactor);

            return NoContent();
        }

        // POST: api/Pricing/apply
        [HttpPost("apply")]
        public ActionResult CalculatePrice([FromBody] ApplyPriceRequest request)
        {
            try
            {
                PriceStrategyType strategy;
                var context = new PriceContext();

                if (request.UseConfiguredStrategy)
                {
                    strategy = _configuration.DefaultStrategy;
                    context.DiscountPercentage = _configuration.DefaultDiscountPercentage;
                    context.DemandFactor = _configuration.DefaultDemandFactor;
                }
                else
                {
                    if (!request.Strategy.HasValue)
                    {
                        return BadRequest(new
                        {
                            message = "Debe especificar Strategy o marcar UseConfiguredStrategy en true."
                        });
                    }

                    strategy = request.Strategy.Value;
                    context.DiscountPercentage = request.DiscountPercentage;
                    context.DemandFactor = request.DemandFactor;
                }

                var finalPrice = _pricingService.CalculatePrice(
                    request.ProductId,
                    strategy,
                    context);

                return Ok(new
                {
                    productId = request.ProductId,
                    strategy = strategy.ToString(),
                    finalPrice
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
