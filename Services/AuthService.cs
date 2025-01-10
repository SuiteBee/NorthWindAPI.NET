using AutoMapper;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Infrastructure;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmployeeRepository _employeeRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IEmployeeRepository employeeRepository, IMapper mapper, ILogger<AuthService> logger)
        {
            _employeeRepository = employeeRepository;

            _mapper = mapper;
            _logger = logger;

        }
        public async Task<AuthDto> Authenticate(string usr, string pwd)
        {
            AuthDto auth = new AuthDto() { UserName = usr, Authorized = false };
            Auth toVerify = await _employeeRepository.GetUser(usr);

            if(toVerify == null)
            {
                return auth;
            }
            else if(toVerify.Hash == null)
            {
                return auth;
            }
            else if(AuthManager.Verify(pwd, toVerify.Hash))
            {
                auth.Authorized = true;
                auth.EmployeeId = toVerify.EmployeeId;
                auth.RoleId = toVerify.RoleId;
                return auth;
            }

            return auth;
        }

        public async Task<bool> ChangePass(string usr, string previous, string pwd)
        {
            Auth user = await _employeeRepository.GetUser(usr);

            if (user == null)
            {
                return false;
            }
            else if (user.Hash == null)
            {
                //Temporary until users have valid pass
                return false;
            }
            else if(AuthManager.Verify(previous, user.Hash))
            {
                var newHash = AuthManager.Hash(pwd);
                user.Hash = newHash;

                var newAuth = await _employeeRepository.UpdateUser(user.Id, user);
                return newAuth != null;
            }

            return false;
        }
    }
}
