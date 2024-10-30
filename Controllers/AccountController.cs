using CRM.Contexts;
using CRM.DTOs.Account;
using CRM.Enums;
using CRM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [Route("api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }

            var (status, token, message) = await _authService.Login(dto);
            if (status == 0)
                return BadRequest(message);

            return Ok(new LoginInfoDTO()
            {
                Message = message,
                Token = token
            });
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }

            var (status, message) = await _authService.Register(dto, RoleType.User.ToString());
            if (status == 0)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }
    }
}
