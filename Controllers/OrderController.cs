using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Services.ResponseDto;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Controllers.Models.Requests;

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
        public async Task<ActionResult<IEnumerable<OrderDto>>> AllOrders()
        {
            var orders = await _orderService.ListOrders();
            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            return orders.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> OrderById(int id)
        {
            var order = await _orderService.FindOrder(id);
            if(order == null)
            {
                return NotFound();
            }
            return order;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Insert(NewOrderRequest newOrder)
        {
            var order = await _orderService.ProcessNewOrder(newOrder);
            return order;
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOrder(int id)
        //{
        //    var order = await _context.Order.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Order.Remove(order);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}
