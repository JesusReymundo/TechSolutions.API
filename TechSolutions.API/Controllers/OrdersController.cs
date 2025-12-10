using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        // GET: api/Orders
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetAll()
        {
            var orders = _orderService.GetAllOrders();
            return Ok(orders);
        }

        // GET: api/Orders/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Order> GetById(Guid id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // POST: api/Orders
        [HttpPost]
        public ActionResult<Order> Create([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = _orderService.CreateOrder(
                request.CustomerName,
                request.Amount,
                request.Description ?? string.Empty
            );

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        // POST: api/Orders/{id}/process
        [HttpPost("{id:guid}/process")]
        public ActionResult<Order> Process(Guid id)
        {
            var order = _orderService.ProcessOrder(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // POST: api/Orders/{id}/cancel
        [HttpPost("{id:guid}/cancel")]
        public ActionResult<Order> Cancel(Guid id)
        {
            var order = _orderService.CancelOrder(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // POST: api/Orders/{id}/discount
        [HttpPost("{id:guid}/discount")]
        public ActionResult<Order> ApplyDiscount(Guid id, [FromBody] ApplyDiscountRequest request)
        {
            var order = _orderService.ApplyDiscount(id, request.Percentage);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // POST: api/Orders/undo
        // Usa Command + Memento para deshacer la última operación de esa orden
        [HttpPost("undo")]
        public ActionResult<Order> UndoLast([FromBody] UndoOrderRequest request)
        {
            var order = _orderService.UndoLast(request.OrderId);
            if (order == null) return NotFound();
            return Ok(order);
        }
    }

    public class CreateOrderRequest
    {
        public string CustomerName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

    public class ApplyDiscountRequest
    {
        public decimal Percentage { get; set; }
    }

    public class UndoOrderRequest
    {
        public Guid OrderId { get; set; }
    }
}
