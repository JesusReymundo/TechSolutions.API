using Microsoft.AspNetCore.Mvc;
using TechSolutions.API.Dtos;
using TechSolutions.Core.Inventory;

namespace TechSolutions.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetAll()
        {
            var items = await _inventoryService.GetAllAsync();
            return Ok(items);
        }

        [HttpPost("adjust")]
        public async Task<ActionResult> AdjustStock(
            [FromBody] AdjustStockRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var (item, notifications) = await _inventoryService.AdjustStockAsync(
                    request.ProductId,
                    request.Delta,
                    cancellationToken);

                return Ok(new { item, notifications });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // ---------- RF6: configurar stock mínimo ----------
        [HttpPut("minimumStock")]
        public async Task<ActionResult<InventoryItem>> UpdateMinimumStock(
            [FromBody] UpdateMinimumStockRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var item = await _inventoryService.UpdateMinimumStockAsync(
                    request.ProductId,
                    request.MinimumStock,
                    cancellationToken);

                return Ok(item);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
