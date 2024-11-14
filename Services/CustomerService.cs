using AutoMapper;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;

            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CustomerDto> FindCustomer(string id)
        {
            var customer = await _customerRepository.FindCustomer(id);
            var toReturn = _mapper.Map<CustomerDto>(customer);
            _mapper.Map(customer, toReturn.ContactInfo);

            return toReturn;
        }

        public async Task<IEnumerable<CustomerDto>> ListCustomers()
        {
            var customers = await _customerRepository.AllCustomers();
            var toReturn = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            
            foreach(CustomerDto dto in toReturn)
            {
                var currentCust = customers.First(x => x.Id == dto.Id);
                _mapper.Map(currentCust, dto.ContactInfo);
            }

            return toReturn;
        }

        public async Task<CustomerDto> ProcessNewCustomer(NewCustomerRequest newCustomer)
        {
            var toInsert = _mapper.Map<Customer>(newCustomer);
            _mapper.Map(newCustomer.ContactInfo, toInsert);
            _mapper.Map(newCustomer.Address, toInsert);

            var inserted = await _customerRepository.InsertCustomer(toInsert);

            return await FindCustomer(inserted.Id);
        }
    }
}
