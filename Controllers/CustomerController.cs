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
    public class CustomerController(ICustomerService customerService, ILogger<UserController> logger) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;
        private readonly ILogger<UserController> _logger = logger;

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> All()
        {
            var customers = await _customerService.ListCustomers();
            if (customers == null || !customers.Any())
            {
                return NotFound();
            }

            return customers.ToList();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> Find(string id)
        {
            var customer = await _customerService.FindCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<RegionsResponse>> Regions()
        {
            var customerRegions = await _customerService.CustomerRegions();
            RegionsResponse response = new() { Regions = customerRegions };
            return response;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create(NewCustomerRequest newCustomer)
        {
            var customer = await _customerService.ProcessNewCustomer(newCustomer);
            return customer;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> Update(string id, CustomerDto customer)
        {
            return await _customerService.Update(id, customer);
        }
    }
}