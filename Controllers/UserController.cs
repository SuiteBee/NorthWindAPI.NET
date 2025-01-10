using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IAuthService authService, IUserService userService, ILogger<UserController> logger)
        {
            _authService = authService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Authenticate(AuthRequest credentials)
        {
            var user = new UserDto() { UserName = credentials.usr };
            var auth = await _authService.Authenticate(credentials.usr, credentials.pwd);

            if (auth.Authorized)
            {
                user = await _userService.FindUser(auth);
                return Accepted(user);
            }

            return Unauthorized(user);
        }

        [HttpPut]
        public async Task<ActionResult> Update(AuthUpdateRequest credentials)
        {
            bool success = await _authService.ChangePass(credentials.usr, credentials.pwd, credentials.newPwd);
            if (success) return Accepted();
            else return Unauthorized();
        }
    }
}