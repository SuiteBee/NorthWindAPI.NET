using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Interfaces
{
    public interface IEmployeeService
    {
        public Task<EmployeeDto> FindEmployee(int id);
        public Task<IEnumerable<EmployeeDto>> ListEmployees();
    }
}
