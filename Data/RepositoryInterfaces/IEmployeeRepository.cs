using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface IEmployeeRepository
    {
        public Task<Employee?> FindEmployee(int id);
        public Task<IEnumerable<Employee>> AllEmployees();
        public Task<Auth> GetUser(string usr);
        public Task<Role> GetRole(int id);
        public Task<Auth?> UpdateUser(int authId, Auth user);
        public Task Save();
    }
}
