using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        public Task<Customer?> FindCustomer(string id);
        public Task<IEnumerable<Customer>> AllCustomers();
        public Task<Customer> InsertCustomer(Customer customer);
        public Task<Customer> UpdateCustomer(string id, Customer customer);
        public Task Save();
    }
}
