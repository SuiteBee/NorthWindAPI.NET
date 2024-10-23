﻿using Microsoft.AspNetCore.Mvc;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Services.Interfaces;

namespace NorthWindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<UserController> _logger;

        public UserController(IAuthService authService, ILogger<UserController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<bool>> Authenticate(AuthRequest credentials)
        {
            bool success = await _authService.Authenticate(credentials.usr, credentials.pwd);
            if (success) return Accepted();
            else return Unauthorized();
        }

        [HttpPut]
        public async Task<IActionResult> Update(AuthUpdateRequest credentials)
        {
            bool success = await _authService.ChangePass(credentials.usr, credentials.pwd, credentials.newPwd);
            if (success) return Accepted();
            else return Unauthorized();
        }
    }
}