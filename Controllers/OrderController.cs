using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Services.ResponseDto;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Controllers.Models.Responses;
using AutoMapper;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IEmployeeService _employeeService;

        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            IOrderService orderService, 
            ICustomerService customerService,
            IEmployeeService employeeService,
            IMapper mapper, 
            ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _customerService = customerService;
            _employeeService = employeeService;

            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> All()
        {
            var orders = await _orderService.ListOrders();
            var customers = await _customerService.ListCustomers();
            var employees = await _employeeService.ListEmployees();

            var orderResponse = _mapper.Map<List<OrderResponse>>(orders);

            foreach(OrderResponse order in orderResponse)
            {
                var customerId = order.OrderedBy.Id;
                order.OrderedBy = customers.First(x => x.Id == customerId);

                var employeeId = order.CompletedBy.Id;
                order.CompletedBy = employees.First(x => x.Id == employeeId);
            }

            if (orderResponse == null || !orderResponse.Any())
            {
                return NotFound();
            }

            return orderResponse;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> Find(int id)
        {
            var order = await _orderService.FindOrder(id);

            var orderResponse = _mapper.Map<OrderResponse>(order);

            orderResponse.OrderedBy = await _customerService.FindCustomer(order.CustomerId);
            orderResponse.CompletedBy = await _employeeService.FindEmployee(order.EmployeeId);

            if(orderResponse == null)
            {
                return NotFound(id);
            }
            return orderResponse;
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
