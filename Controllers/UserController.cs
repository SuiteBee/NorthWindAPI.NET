using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Services.Interfaces;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class UserController(IAuthService authService, IUserService userService, ILogger<UserController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IUserService _userService = userService;
        private readonly ILogger<UserController> _logger = logger;

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Authenticate(AuthRequest credentials)
        {
            var response = new UserResponse();
            var auth = await _authService.Authenticate(credentials.Usr, credentials.Pwd);

            if (auth.Authorized)
            {
                response.Token = _authService.GenerateToken();
                response.AuthorizedUser = await _userService.FindUser(auth);
                return Accepted(response);
            }

            return Unauthorized(response);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Update(AuthUpdateRequest credentials)
        {
            bool success = await _authService.ChangePass(credentials.Usr, credentials.Pwd, credentials.NewPwd);
            if (success) return Accepted();
            else return Unauthorized();
        }
    }
}