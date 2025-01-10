using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Interfaces
{
    public interface IUserService
    {
        public Task<EmployeeDto> FindEmployee(int id);
        public Task<UserDto> FindUser(AuthDto auth);
        public Task<IEnumerable<EmployeeDto>> ListEmployees();
    }
}
