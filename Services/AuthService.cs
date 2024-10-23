using AutoMapper;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Infrastructure;
using NorthWindAPI.Services.Interfaces;

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
        public async Task<bool> Authenticate(string usr, string pwd)
        {
            Auth toVerify = await _employeeRepository.GetUser(usr);

            if(toVerify == null)
            {
                return false;
            }
            else if(toVerify.Hash == null)
            {
                return false;
            }
            else
            {
                return AuthManager.Verify(pwd, toVerify.Hash);
            }
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
