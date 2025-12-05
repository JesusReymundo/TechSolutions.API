using Microsoft.AspNetCore.Mvc;
using TechSolutions.API.Dtos;
using TechSolutions.Core.Orders;

namespace TechSolutions.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetAll()
        {
            var orders = _orderService.GetAll();
            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Order> GetById(int id)
        {
            try
            {
                var order = _orderService.GetById(id);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<Order> Create([FromBody] CreateOrderRequest request)
        {
            var order = _orderService.CreateOrder(request.CustomerName, request.Amount);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPost("{id:int}/process")]
        public ActionResult<Order> Process(int id)
        {
            try
            {
                var order = _orderService.Process(id);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id:int}/discount")]
        public ActionResult<Order> ApplyDiscount(int id, [FromBody] ApplyOrderDiscountRequest request)
        {
            try
            {
                var order = _orderService.ApplyDiscount(id, request.DiscountPercentage);
                return Ok(order);
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

        [HttpPost("{id:int}/cancel")]
        public ActionResult<Order> Cancel(int id)
        {
            try
            {
                var order = _orderService.Cancel(id);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id:int}/undo")]
        public ActionResult<Order> UndoLast(int id)
        {
            try
            {
                var success = _orderService.UndoLast(id);
                if (!success)
                    return BadRequest(new { message = "No hay más cambios para deshacer en este pedido." });

                var order = _orderService.GetById(id);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
