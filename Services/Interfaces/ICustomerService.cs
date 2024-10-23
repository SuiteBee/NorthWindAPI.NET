using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<CustomerDto> FindCustomer(string id);
        public Task<IEnumerable<CustomerDto>> ListCustomers();
        public Task<CustomerDto> ProcessNewCustomer(NewCustomerRequest newCustomer);
    }
}
