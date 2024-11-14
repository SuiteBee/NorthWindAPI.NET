using AutoMapper;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;

            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EmployeeDto> FindEmployee(int id)
        {
            var employee = await _employeeRepository.FindEmployee(id);
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<IEnumerable<EmployeeDto>> ListEmployees()
        {
            var employees = await _employeeRepository.AllEmployees();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }
    }
}