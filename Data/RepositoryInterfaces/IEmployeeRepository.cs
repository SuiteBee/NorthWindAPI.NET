using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface IEmployeeRepository
    {
        public Task<Employee?> FindEmployee(int id);
    }
}
