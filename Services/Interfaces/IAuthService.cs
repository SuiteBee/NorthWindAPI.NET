using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthDto> Authenticate(string usr, string pwd);
        public string GenerateToken();

        public Task<bool> ChangePass(string usr, string previous, string pwd);
    }
}
