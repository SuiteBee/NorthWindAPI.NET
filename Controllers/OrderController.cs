using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Services;
using NorthWindAPI.Services.Dto;
using NorthWindAPI.Services.Interfaces;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Order>>> GetOrdersAll()
        //{
        //    return await _context.Order.ToListAsync();
        //}

        //[HttpGet]
        //public async Task<IEnumerable<Order>> GetOrdersGeneric()
        //{
        //    var baseOrder =  new BaseRepository<Order>(_context);
        //    return await baseOrder.ReturnEntityListAsync();
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var order = await _orderService.FindOrder(id);
            if(order == null)
            {
                return NotFound();
            }
            return order;
        }

        //[HttpPost]
        //public async Task<ActionResult<Order>> PostOrder(Order newOrder)
        //{
        //    _context.Order.Add(newOrder);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetOrderById), new { id = newOrder.Id }, newOrder);
        //}

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
