using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Authentication;
using Ogani.WebApp.Business.DTOs.Auth;
using Ogani.WebApp.Business.Exceptions;

namespace Ogani.WebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, IJwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _authService.RegisterAsync(dto);
                return Ok(new { Message = "User registered successfully." });
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _authService.ValidateUserAsync(dto);

            if (user is null)
                return Unauthorized(new { Message = "Invalid email or password." });

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(60),
                User = new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    user.Roles
                }
            });
        }
    }
}