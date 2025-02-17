using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Infrastructure.Exceptions.Repository;
using NorthWindAPI.Services.Interfaces;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class OrderController(
        IOrderService orderService,
        ICustomerService customerService,
        IProductService productService,
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
            try
            {
                var orders = await _orderService.ListOrders();
                var customers = await _customerService.ListCustomers();
                var employees = await _employeeService.ListEmployees();

                //Map initial Order list
                var orderResponse = _mapper.Map<List<OrderResponse>>(orders);
                var customerResponse = _mapper.Map<List<CustomerResponse>>(customers);
                var employeeResponse = _mapper.Map<List<EmployeeResponse>>(employees);

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
            catch(OrderNotFoundException ex) 
            {
                return NotFound(ex.Message);
            }
            catch(CustomerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(EmployeeNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
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
            catch (OrderNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (CustomerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (EmployeeNotFoundException ex)
            {
                return NotFound(ex.Message);
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
                var orderResponse = _mapper.Map<OrderResponse>(order);

                var customer = await _customerService.FindCustomer(order.CustomerId);
                var employee = await _employeeService.FindEmployee(order.EmployeeId);

                orderResponse.OrderedBy = _mapper.Map<CustomerResponse>(customer);
                orderResponse.CompletedBy = _mapper.Map<EmployeeResponse>(employee);

                return orderResponse;
            }
            catch(OrderNotCreatedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (OrderDetailNotCreatedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (CustomerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (EmployeeNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ProductNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ProductNotUpdatedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Forbid(ex.Message);
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
                var orderResponse = _mapper.Map<OrderResponse>(order);

                var customer = await _customerService.FindCustomer(order.CustomerId);
                var employee = await _employeeService.FindEmployee(order.EmployeeId);

                orderResponse.OrderedBy = _mapper.Map<CustomerResponse>(customer);
                orderResponse.CompletedBy = _mapper.Map<EmployeeResponse>(employee);

                return orderResponse;
            }
            catch (OrderNotUpdatedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (OrderNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (CustomerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (EmployeeNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Forbid(ex.Message);
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
            catch (OrderNotUpdatedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (OrderNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (CustomerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (EmployeeNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Forbid(ex.Message);
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
            try
            {
                var carriers = await _orderService.Carriers();

                ShipOptionResponse response = new() { Carriers = carriers };
                return response;
            }
            catch(CarrierNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Forbid(ex.Message);
            }
        }

        /// <summary>
        /// Delete order record + order detail record
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _orderService.RemoveOrder(id);

                return NoContent();
            }
            catch(OrderNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(OrderNotRemovedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(ProductNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(ProductNotUpdatedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}
