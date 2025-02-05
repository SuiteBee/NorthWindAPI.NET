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
            var customers = await _customerService.ListCustomers();
            if (customers == null || !customers.Any())
            {
                return NotFound();
            }

            var response = _mapper.Map<IEnumerable<CustomerResponse>>(customers);
            return response.ToList();
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
            var customer = await _customerService.FindCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<CustomerResponse>(customer);
            return response;
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
            var customerRegions = await _customerService.CustomerRegions();
            if (customerRegions == null || !customerRegions.Any())
            {
                return NotFound();
            }

            RegionsResponse response = new() { Regions = customerRegions };
            return response;
        }

        /// <summary>
        /// Insert new customer record
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
            var customer = new CustomerDto();
            try
            {
                customer = await _customerService.ProcessNewCustomer(newCustomer);

                if (customer == null)
                {
                    return BadRequest();
                }
            }
            catch
            {
                return Forbid();
            }

            var response = _mapper.Map<CustomerResponse>(customer);
            return response;
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
            var customer = new CustomerDto();

            try
            {
                var toUpdate = _mapper.Map<CustomerDto>(existingCustomer);

                if (id != toUpdate.Id)
                {
                    return BadRequest();
                }

                customer = await _customerService.Update(id, toUpdate);
            }
            catch
            {
                return Forbid();
            }

            var response = _mapper.Map<CustomerResponse>(customer);
            return response;
        }
    }
}