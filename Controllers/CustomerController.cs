using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<UserController> _logger;

        public CustomerController(ICustomerService customerService, ILogger<UserController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

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

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create(NewCustomerRequest newCustomer)
        {
            var customer = await _customerService.ProcessNewCustomer(newCustomer);
            return customer;
        }
    }
}