using AutoMapper;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Infrastructure.Exceptions.Repository;
using NorthWindAPI.Infrastructure.Exceptions.Service;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services
{
    public class CustomerService(ICustomerRepository customerRepository, IMapper mapper, ILogger<CustomerService> logger) : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;

        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CustomerService> _logger = logger;

        #region " SELECT "

        public async Task<CustomerDto> FindCustomer(string id)
        {
            try
            {
                var customer = await _customerRepository.FindCustomer(id);
                var toReturn = _mapper.Map<CustomerDto>(customer);
                _mapper.Map(customer, toReturn.ContactInfo);
                _mapper.Map(customer, toReturn.Address);

                return toReturn;
            }
            catch(EntityNotFoundException ex)
            {
                throw new CustomerNotFoundException($"Customer {id} not found");
            }
        }

        public async Task<IEnumerable<CustomerDto>> ListCustomers()
        {
            var customers = await _customerRepository.AllCustomers();
            var toReturn = _mapper.Map<IEnumerable<CustomerDto>>(customers);

            foreach (CustomerDto dto in toReturn)
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

        #endregion

        #region " INSERT "

        public async Task<CustomerDto> ProcessNewCustomer(CustomerRequest newCustomer)
        {
            var toInsert = _mapper.Map<Customer>(newCustomer);
            _mapper.Map(newCustomer.ContactInfo, toInsert);
            _mapper.Map(newCustomer.Address, toInsert);

            var inserted = await _customerRepository.InsertCustomer(toInsert);
            await _customerRepository.Save();

            return await FindCustomer(inserted.Id);
        }

        #endregion

        #region " UPDATE "

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
            await _customerRepository.Save();

            return await FindCustomer(id);
        }

        #endregion
    }
}
