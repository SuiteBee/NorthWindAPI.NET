using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers.Models.Responses
{
    public class UserResponse
    {
        public UserDto AuthorizedUser { get; set; } = new UserDto();
    }
}
