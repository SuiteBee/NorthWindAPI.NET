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
            _mapper.Map(customer, toReturn.Address);

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
                _mapper.Map(currentCust, dto.Address);
            }

            return toReturn;
        }

        public async Task<IEnumerable<RegionDto>> CustomerRegions()
        {
            var customers = await _customerRepository.AllCustomers();

            var countryRegions = customers.Select(c => new RegionDto { Region = c.Region, Country = c.Country });
            var uniqueCountryRegions = countryRegions.DistinctBy(r => new { r.Region, r.Country });

            return uniqueCountryRegions.ToList();
        }

        public async Task<CustomerDto> ProcessNewCustomer(NewCustomerRequest newCustomer)
        {
            var toInsert = _mapper.Map<Customer>(newCustomer);
            _mapper.Map(newCustomer.ContactInfo, toInsert);
            _mapper.Map(newCustomer.Address, toInsert);

            var inserted = await _customerRepository.InsertCustomer(toInsert);

            return await FindCustomer(inserted.Id);
        }

        public async Task<CustomerDto> Update(string id, CustomerDto customer)
        {
            var cusBase = await _customerRepository.FindCustomer(id);

            //Cannot be mapped due to tracking
            cusBase.Address = customer.Address.Street;
            cusBase.City = customer.Address.City;
            cusBase.PostalCode = customer.Address.PostalCode;
            cusBase.Country = customer.Address.Country;
            cusBase.Region = customer.Address.Region;

            cusBase.ContactName = customer.ContactInfo.ContactName;
            cusBase.ContactTitle = customer.ContactInfo.ContactTitle;
            cusBase.Phone = customer.ContactInfo.Phone;
            cusBase.Fax = customer.ContactInfo.Fax;


            await _customerRepository.UpdateCustomer(id, cusBase);
            return await FindCustomer(id);
        }
    }
}
