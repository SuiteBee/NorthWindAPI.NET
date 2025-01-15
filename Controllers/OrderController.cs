using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class OrderController(
        IOrderService orderService,
        ICustomerService customerService,
        IUserService employeeService,
        IMapper mapper,
        ILogger<OrderController> logger
    ) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;
        private readonly ICustomerService _customerService = customerService;
        private readonly IUserService _employeeService = employeeService;

        private readonly IMapper _mapper = mapper;
        private readonly ILogger<OrderController> _logger = logger;

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> All()
        {
            var orders = await _orderService.ListOrders();
            var customers = await _customerService.ListCustomers();
            var employees = await _employeeService.ListEmployees();

            var orderResponse = _mapper.Map<List<OrderResponse>>(orders);

            foreach (OrderResponse order in orderResponse)
            {
                var customerId = order.OrderedBy.Id;
                order.OrderedBy = customers.First(x => x.Id == customerId);

                var employeeId = order.CompletedBy.Id;
                order.CompletedBy = employees.First(x => x.Id == employeeId);
            }

            if (orderResponse == null || orderResponse.Count == 0)
            {
                return NotFound();
            }

            return orderResponse;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> Find(int id)
        {
            try
            {
                var order = await _orderService.FindOrder(id);

                var orderResponse = _mapper.Map<OrderResponse>(order);

                orderResponse.OrderedBy = await _customerService.FindCustomer(order.CustomerId);
                orderResponse.CompletedBy = await _employeeService.FindEmployee(order.EmployeeId);

                return orderResponse;
            }
            catch
            {
                return NotFound(id);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> Create(NewOrderRequest newOrder)
        {
            var order = await _orderService.ProcessNewOrder(newOrder);

            var orderResponse = _mapper.Map<OrderResponse>(order);

            orderResponse.OrderedBy = await _customerService.FindCustomer(order.CustomerId);
            orderResponse.CompletedBy = await _employeeService.FindEmployee(order.EmployeeId);

            return orderResponse;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderResponse>> Ship(int id, string? shipDate = null)
        {
            var order = await _orderService.MarkAsShipped(id, shipDate);

            var orderResponse = _mapper.Map<OrderResponse>(order);

            orderResponse.OrderedBy = await _customerService.FindCustomer(order.CustomerId);
            orderResponse.CompletedBy = await _employeeService.FindEmployee(order.EmployeeId);

            return orderResponse;
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<IEnumerable<OrderDto>>> ShipMany(ShipRequest orders)
        {
            var shippedOrders = await _orderService.MarkAsShipped(orders);
            return shippedOrders.ToList();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ShipOptionResponse>> Carriers()
        {
            var carriers = await _orderService.Carriers();
            ShipOptionResponse response = new() { Carriers = carriers };
            return response;
        }

        [Authorize]
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
