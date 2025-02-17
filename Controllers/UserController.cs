using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Infrastructure.Exceptions.Repository;
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

        /// <summary>
        /// Validate user credentials + Return user info and token
        /// </summary>
        /// <param name="credentials"></param>
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<ActionResult<UserResponse>> Authenticate(AuthRequest credentials)
        {
            try
            {
                if (string.IsNullOrEmpty(credentials.Usr) || string.IsNullOrEmpty(credentials.Pwd))
                {
                    return BadRequest();
                }

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
            catch(UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
           
        }

        /// <summary>
        /// Validate user credentials + Update user credentials
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(AcceptedResult), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Update(AuthUpdateRequest credentials)
        {
            try
            {
                bool success = await _authService.ChangePass(credentials.Usr, credentials.Pwd, credentials.NewPwd);
                if (success) return Accepted();
                else return Unauthorized();
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UserNotUpdatedException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}