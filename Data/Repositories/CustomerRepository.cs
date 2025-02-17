using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Infrastructure.Exceptions.Base;
using NorthWindAPI.Infrastructure.Exceptions.Repository;

namespace NorthWindAPI.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        private readonly IBaseAltRepository<Customer> _baseCustomerRepo;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
            _baseCustomerRepo = new BaseAltRepository<Customer>(_context);
        }

        public async Task<Customer?> FindCustomer(string id)
        {
            try
            {
                return await _baseCustomerRepo.FindEntityAsync(id);
            }
            catch (EntityNotFoundException ex)
            {
                throw new CustomerNotFoundException($"Customer {ex.Message}");
            }
        }

        public async Task<IEnumerable<Customer>> AllCustomers()
        {
            try
            {
                return await _baseCustomerRepo.ReturnEntityListAsync();
            }
            catch (EntityNotFoundException ex)
            {
                throw new CustomerNotFoundException($"Customers {ex.Message}");
            }

        }

        public Customer InsertCustomer(Customer customer)
        {
            try
            {
                return _baseCustomerRepo.AddEntity(customer);
            }
            catch (EntityNotCreatedException ex)
            {
                throw new CustomerNotCreatedException($"Customer {ex.Message}");
            }
        }

        public Customer UpdateCustomer(string id, Customer customer)
        {
            try
            {
                return _baseCustomerRepo.UpdateEntity(id, customer);
            }
            catch (EntityNotUpdatedException ex)
            {
                throw new CustomerNotUpdatedException($"Customer {ex.Message}");
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
