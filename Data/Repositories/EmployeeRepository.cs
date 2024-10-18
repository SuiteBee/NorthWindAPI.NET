using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        private IBaseRepository<Employee> _baseEmployeeRepo;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
            _baseEmployeeRepo = new BaseRepository<Employee>(_context);
        }

        public async Task<Employee?> FindEmployee(int id)
        {
            return await _baseEmployeeRepo.FindEntityAsync(id);
        }
    }
}
