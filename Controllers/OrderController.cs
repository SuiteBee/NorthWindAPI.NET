using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Services.Interfaces;

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

        /// <summary>
        /// Get list of all existing orders
        /// </summary>
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> All()
        {
            var orders = await _orderService.ListOrders();
            var customers = await _customerService.ListCustomers();
            var employees = await _employeeService.ListEmployees();

            //Map initial Order list
            var orderResponse = _mapper.Map<List<OrderResponse>>(orders);
            var customerResponse = _mapper.Map<List<CustomerResponse>>(customers);
            var employeeResponse = _mapper.Map<List<EmployeeResponse>>(employees);

            if (orderResponse == null || orderResponse.Count == 0)
            {
                return NotFound();
            }

            //Join customers on ID and return order list with customer info complete
            orderResponse = orderResponse.Join(customerResponse, order => order.OrderedBy.Id, customer => customer.Id, (order, customer) =>
            {
                order.OrderedBy = customer;
                return order;
            }).ToList();

            //Join employees on ID and return order list with employee info complete
            orderResponse = orderResponse.Join(employeeResponse, order => order.CompletedBy.Id, employee => employee.Id, (order, employee) =>
            {
                order.CompletedBy = employee;
                return order;
            }).ToList();

            //Opt for linq joins over loop and search
            //foreach (OrderResponse order in orderResponse)
            //{
            //    var customerId = order.OrderedBy.Id;
            //    order.OrderedBy = customers.First(x => x.Id == customerId);

            //    var employeeId = order.CompletedBy.Id;
            //    order.CompletedBy = employees.First(x => x.Id == employeeId);
            //}

            return orderResponse;
        }

        /// <summary>
        /// Get single order record by ID
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> Find(int id)
        {
            try
            {
                var order = await _orderService.FindOrder(id);
                var orderResponse = _mapper.Map<OrderResponse>(order);
                
                var customer = await _customerService.FindCustomer(order.CustomerId);
                var employee = await _employeeService.FindEmployee(order.EmployeeId);

                orderResponse.OrderedBy = _mapper.Map<CustomerResponse>(customer);
                orderResponse.CompletedBy = _mapper.Map<EmployeeResponse>(employee);

                return orderResponse;
            }
            catch
            {
                return NotFound(id);
            }
        }

        /// <summary>
        /// Insert new order record + Return inserted record
        /// </summary>
        /// <param name="newOrder"></param>
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ForbidResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> Create(NewOrderRequest newOrder)
        {
            try
            {
                var order = await _orderService.ProcessNewOrder(newOrder);

                if (order == null)
                {
                    return BadRequest();
                }

                var orderResponse = _mapper.Map<OrderResponse>(order);

                var customer = await _customerService.FindCustomer(order.CustomerId);
                var employee = await _employeeService.FindEmployee(order.EmployeeId);

                orderResponse.OrderedBy = _mapper.Map<CustomerResponse>(customer);
                orderResponse.CompletedBy = _mapper.Map<EmployeeResponse>(employee);

                return orderResponse;
            }
            catch
            {
                return Forbid();
            }
        }

        /// <summary>
        /// Update order record ship date + Return order record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shipDate"></param>
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ForbidResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderResponse>> Ship(int id, string? shipDate = null)
        {
            try
            {
                var order = await _orderService.MarkAsShipped(id, shipDate);

                if (order == null)
                {
                    return BadRequest();
                }

                var orderResponse = _mapper.Map<OrderResponse>(order);

                var customer = await _customerService.FindCustomer(order.CustomerId);
                var employee = await _employeeService.FindEmployee(order.EmployeeId);

                orderResponse.OrderedBy = _mapper.Map<CustomerResponse>(customer);
                orderResponse.CompletedBy = _mapper.Map<EmployeeResponse>(employee);

                return orderResponse;
            }
            catch
            {
                return Forbid();
            }
        }

        /// <summary>
        /// Update multiple order shipping dates + Return collection of orders
        /// </summary>
        /// <param name="orders"></param>
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ForbidResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpPut]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> ShipMany(ShipRequest orders)
        {
            try
            {
                var shippedOrders = await _orderService.MarkAsShipped(orders);

                var customers = await _customerService.ListCustomers();
                var employees = await _employeeService.ListEmployees();

                //Map initial Order list
                var orderResponse = _mapper.Map<List<OrderResponse>>(shippedOrders);
                var customerResponse = _mapper.Map<List<CustomerResponse>>(customers);
                var employeeResponse = _mapper.Map<List<EmployeeResponse>>(employees);

                if (orderResponse == null || orderResponse.Count == 0)
                {
                    return NotFound();
                }

                //Join customers on ID and return order list with customer info complete
                orderResponse = orderResponse.Join(customerResponse, order => order.OrderedBy.Id, customer => customer.Id, (order, customer) =>
                {
                    order.OrderedBy = customer;
                    return order;
                }).ToList();

                //Join employees on ID and return order list with employee info complete
                orderResponse = orderResponse.Join(employeeResponse, order => order.CompletedBy.Id, employee => employee.Id, (order, employee) =>
                {
                    order.CompletedBy = employee;
                    return order;
                }).ToList();

                return orderResponse;
            }
            catch
            {
                return Forbid();
            }
        }

        /// <summary>
        /// Get list of all available carriers
        /// </summary>
        [ProducesResponseType(typeof(ShipOptionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ShipOptionResponse>> Carriers()
        {
            var carriers = await _orderService.Carriers();

            if (carriers == null || carriers.ToList().Count == 0)
            {
                return NotFound();
            }

            ShipOptionResponse response = new() { Carriers = carriers };
            return response;
        }

        /// <summary>
        /// Delete order record + order detail record
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
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
