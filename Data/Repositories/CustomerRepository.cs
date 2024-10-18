using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        private IBaseAltRepository<Customer> _baseCustomerRepo;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
            _baseCustomerRepo = new BaseAltRepository<Customer>(_context);
        }

        public async Task<Customer?> FindCustomer(string id)
        {
            return await _baseCustomerRepo.FindEntityAsync(id);
        }
    }
}
