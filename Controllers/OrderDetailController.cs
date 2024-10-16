using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.Models;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderDetailController> _logger;

        public OrderDetailController(AppDbContext context, ILogger<OrderDetailController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetailsAll()
        {
            return await _context.OrderDetail.ToListAsync();
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetailById(int orderId)
        {
            var detail = await _context.OrderDetail.FirstOrDefaultAsync(x => x.OrderId == orderId);
            if (detail == null)
            {
                return NotFound();
            }
            return detail;
        }
    }
}
