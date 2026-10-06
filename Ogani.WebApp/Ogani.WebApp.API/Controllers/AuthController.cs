using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Ogani.WebApp.API.Extensions;
using Ogani.WebApp.Business.Authentication;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.DTOs.Auth;
using System.Security.Claims;

namespace Ogani.WebApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthsController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly JwtOptions _jwtOptions;

    public AuthsController(IAuthService authService, IOptions<JwtOptions> jwtOptions)
    {
        _authService = authService;
        _jwtOptions = jwtOptions.Value;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        try
        {
            AppUserDTO user = await _authService.RegisterAsync(dto);
            string token = _jwtOptions.GenerateToken(user);

            await SendEmailConfirmationAsync(user.Id);

            return Ok(new
            {
                Message = "User registered successfully.",
                Token = token,
                User = user
            });
        }
        catch (BusinessValidationException ex)
        {
            return BadRequest(new { Errors = ex.Errors });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        try
        {
            AppUserDTO user = await _authService.ValidateUserAsync(dto);
            string token = _jwtOptions.GenerateToken(user);

            return Ok(new
            {
                Message = "Login successful.",
                Token = token,
                User = user
            });
        }
        catch (BusinessValidationException ex)
        {
            return BadRequest(new { Errors = ex.Errors });
        }
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        ProfileUpdateDTO userProfile = await _authService.GetUserProfileAsync(userId);

        return Ok(userProfile);
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateDTO dto)
    {
        dto.Id = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        try
        {
            await _authService.UpdateUserProfileAsync(dto);

            AppUserDTO user = new()
            {
                Id = dto.Id,
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                IsEmailConfirmed = bool.Parse(User.FindFirstValue("IsEmailConfirmed") ?? "false"),
                Roles = User.FindAll(ClaimTypes.Role).Select(claim => claim.Value).ToList()
            };

            if (dto.Email != User.FindFirstValue(ClaimTypes.Email))
            {
                user.IsEmailConfirmed = false;
                await SendEmailConfirmationAsync(dto.Id);
            }

            string token = _jwtOptions.GenerateToken(user);

            return Ok(new
            {
                Message = "Profile updated successfully.",
                Token = token,
                User = user
            });
        }
        catch (BusinessValidationException ex)
        {
            return BadRequest(new { Errors = ex.Errors });
        }
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeDTO dto)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        try
        {
            await _authService.ChangePasswordAsync(userId, dto);
            return Ok(new { Message = "Password changed successfully." });
        }
        catch (BusinessValidationException ex)
        {
            return BadRequest(new { Errors = ex.Errors });
        }
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
    {
        try
        {
            string resetPasswordUrl = Url.Action(nameof(ResetPassword), "Auths", null, Request.Scheme)!;
            await _authService.SendPasswordResetEmailAsync(dto, resetPasswordUrl);

            return Ok(new { Message = "If the email is registered, a password reset link has been sent." });
        }
        catch (BusinessValidationException ex)
        {
            return BadRequest(new { Errors = ex.Errors });
        }
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
    {
        try
        {
            await _authService.ResetPasswordAsync(dto);
            return Ok(new { Message = "Password has been reset successfully." });
        }
        catch (BusinessValidationException ex)
        {
            return BadRequest(new { Errors = ex.Errors });
        }
    }

    [HttpPost("send-email-confirmation")]
    [Authorize]
    public async Task<IActionResult> SendEmailConfirmation()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        bool isSent = await SendEmailConfirmationAsync(userId);
        if (!isSent)
            return BadRequest(new { Message = "Failed to send confirmation email." });

        return Ok(new { Message = "Confirmation email sent successfully." });
    }

    [HttpGet("confirm-email")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            return BadRequest(new { Message = "Invalid email confirmation parameters." });

        try
        {
            await _authService.ConfirmEmailAsync(userId, token);
            return Ok(new { Message = "Email confirmed successfully." });
        }
        catch (BusinessValidationException ex)
        {
            return BadRequest(new { Errors = ex.Errors });
        }
    }

    #region Helper Methods

    private async Task<bool> SendEmailConfirmationAsync(string userId)
    {
        try
        {
            string confirmEmailUrl = Url.Action(nameof(ConfirmEmail), "Auths", null, Request.Scheme)!;
            await _authService.SendEmailConfirmationAsync(userId, confirmEmailUrl);
            return true;
        }
        catch (BusinessException)
        {
            return false;
        }
    }

    #endregion
}