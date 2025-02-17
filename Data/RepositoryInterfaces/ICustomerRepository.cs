using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        public Task<Customer?> FindCustomer(string id);
        public Task<IEnumerable<Customer>> AllCustomers();
        public Customer InsertCustomer(Customer customer);
        public Customer UpdateCustomer(string id, Customer customer);
        public Task Save();
    }
}
