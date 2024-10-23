using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        private readonly IBaseRepository<Employee> _baseEmployeeRepo;
        private readonly IBaseRepository<Auth> _baseAuthRepo;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
            _baseEmployeeRepo = new BaseRepository<Employee>(_context);
            _baseAuthRepo = new BaseRepository<Auth>(_context);
        }
        private IQueryable<Auth> QueryAuth()
        {
            return _context.Auth.AsQueryable();
        }

        public async Task<Employee?> FindEmployee(int id)
        {
            return await _baseEmployeeRepo.FindEntityAsync(id);
        }

        public async Task<IEnumerable<Employee>> AllEmployees()
        {
            return await _baseEmployeeRepo.ReturnEntityListAsync();
        }

        public async Task<Auth> GetUser(string usr)
        {
            return await QueryAuth().Where(x => x.Username == usr).FirstAsync();
        }

        public async Task<Auth?> UpdateUser(int authId, Auth user)
        {
            return await _baseAuthRepo.UpdateEntityAsync(authId, user);
        }
    }
}
