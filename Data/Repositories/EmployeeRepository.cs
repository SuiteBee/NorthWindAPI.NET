using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Context;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Infrastructure.Exceptions.Base;
using NorthWindAPI.Infrastructure.Exceptions.Repository;

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

        private IQueryable<Role> QueryRole()
        {
            return _context.Role.AsQueryable();
        }

        public async Task<Employee?> FindEmployee(int id)
        {
            try
            {
                return await _baseEmployeeRepo.FindEntityAsync(id);
            }
            catch(EntityNotFoundException ex)
            {
                throw new EmployeeNotFoundException($"Employee {ex.Message}");
            }
        }

        public async Task<IEnumerable<Employee>> AllEmployees()
        {
            try
            {
                return await _baseEmployeeRepo.ReturnEntityListAsync();
            }
            catch(EntityNotFoundException ex)
            {
                throw new EmployeeNotFoundException($"Employees {ex.Message}");
            }
        }

        public async Task<Auth> GetUser(string usr)
        {
            var users = QueryAuth().Where(x => x.Username == usr);
            if(users == null || !users.Any())
            {
                throw new UserNotFoundException($"User {usr} not found");
            }
            return await users.FirstAsync();
        }

        public async Task<Role> GetRole(int id)
        {
            var roles = QueryRole().Where(x => x.Id == id);
            if(roles == null || !roles.Any())
            {
                throw new RoleNotFoundException($"Role {id} not found");
            }
            return await roles.FirstAsync();
        }

        public Auth UpdateUser(int authId, Auth user)
        {
            try
            {
                return _baseAuthRepo.UpdateEntity(authId, user);
            }
            catch (EntityNotUpdatedException ex)
            {
                throw new UserNotUpdatedException($"User {ex.Message}");
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
