using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Infrastructure.Exceptions.Base;
using NorthWindAPI.Infrastructure.Exceptions.Repository;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class CustomerController(ICustomerService customerService, IMapper mapper, ILogger<UserController> logger) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UserController> _logger = logger;
        
        /// <summary>
        /// Get list of all existing customer records
        /// </summary>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> All()
        {
            try
            {
                var customers = await _customerService.ListCustomers();
                var response = _mapper.Map<IEnumerable<CustomerResponse>>(customers);
                return response.ToList();
            }
            catch(CustomerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get single customer record by ID
        /// </summary>
        /// <param name="id">RBEE</param>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CustomerResponse>> Find(string id)
        {
            try
            {
                var customer = await _customerService.FindCustomer(id);
                var response = _mapper.Map<CustomerResponse>(customer);
                return response;
            }
            catch(CustomerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get list of unique customer global regions
        /// </summary>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(RegionsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RegionsResponse>> Regions()
        {
            try
            {
                var customerRegions = await _customerService.CustomerRegions();
                RegionsResponse response = new() { Regions = customerRegions };
                return response;
            }
            catch(CustomerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Insert new customer record + Return inserted record
        /// </summary>
        /// <param name="newCustomer"></param>
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ForbidResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CustomerResponse>> Create(CustomerRequest newCustomer)
        {
            try
            {
                var customer = await _customerService.ProcessNewCustomer(newCustomer);

                var response = _mapper.Map<CustomerResponse>(customer);
                return response;
            }
            catch(CustomerNotCreatedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return Forbid(ex.Message);
            }
        }

        /// <summary>
        /// Update existing customer record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="existingCustomer"></param>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ForbidResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CustomerResponse>> Update(string id, CustomerRequest existingCustomer)
        {
            try
            {
                var toUpdate = _mapper.Map<CustomerDto>(existingCustomer);
                var customer = await _customerService.Update(id, toUpdate);

                var response = _mapper.Map<CustomerResponse>(customer);
                return response;
            }
            catch(CustomerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(CustomerNotUpdatedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(EntityIdentifierMismatchException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}