﻿using AutoMapper;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IEmployeeRepository _employeeRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IEmployeeRepository employeeRepository, IMapper mapper, ILogger<UserService> logger)
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

        public async Task<UserDto> FindUser(AuthDto auth)
        {
            var employee = await _employeeRepository.FindEmployee(auth.EmployeeId);
            var role = await _employeeRepository.GetRole(auth.RoleId);

            var user = new UserDto() { UserName = auth.UserName };
            _mapper.Map(employee, user);
            _mapper.Map(role, user);

            return user;
        }

        public async Task<IEnumerable<EmployeeDto>> ListEmployees()
        {
            var employees = await _employeeRepository.AllEmployees();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }
    }
}