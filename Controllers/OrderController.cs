using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Services.ResponseDto;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Services;
using System.Runtime.InteropServices;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> All()
        {
            var orders = await _orderService.ListOrders();
            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            return orders.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> Find(int id)
        {
            var order = await _orderService.FindOrder(id);
            if(order == null)
            {
                return NotFound(id);
            }
            return order;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create(NewOrderRequest newOrder)
        {
            var order = await _orderService.ProcessNewOrder(newOrder);
            return order;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> Ship(int id, string? shipDate = null)
        {
            return await _orderService.MarkAsShipped(id, shipDate);
        }

        [HttpPut]
        public async Task<ActionResult<IEnumerable<OrderDto>>> ShipMany(ShipRequest orders)
        {
            var shippedOrders = await _orderService.MarkAsShipped(orders);
            return shippedOrders.ToList();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _orderService.RemoveOrder(id);
            if (!result)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
