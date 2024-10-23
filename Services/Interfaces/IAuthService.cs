using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<bool> Authenticate(string usr, string pwd);

        public Task<bool> ChangePass(string usr, string previous, string pwd);
    }
}
