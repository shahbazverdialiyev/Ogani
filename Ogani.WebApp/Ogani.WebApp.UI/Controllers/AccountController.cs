using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Authentication;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.DTOs.Auth;
using Ogani.WebApp.UI.Extensions;
using Ogani.WebApp.UI.Models.Account;
using System.Security.Claims;

namespace Ogani.WebApp.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                AppUserDTO user = await _authService.RegisterAsync(model);
                await HttpContext.SignInUserAsync(user, isPersistent: true);

                await SendEmailConfirmationAsync(user.Id);
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            TempData["SuccessMessage"] = "Account created successfully.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                AppUserDTO user = await _authService.ValidateUserAsync(model);
                await HttpContext.SignInUserAsync(user, isPersistent: model.RememberMe);
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            TempData["SuccessMessage"] = "Login successfully.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Profile()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            ProfileUpdateDTO user = await _authService.GetUserProfileAsync(id);

            return View(user);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileUpdateDTO dto)
        {
            dto.Id = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (!ModelState.IsValid)
                return View(dto);

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

                await HttpContext.SignInUserAsync(user, isPersistent: true);
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(dto);
            }

            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet, Authorize]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(PasswordChangeVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            try
            {
                await _authService.ChangePasswordAsync(userId, model);
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(model);
            }

            TempData["SuccessMessage"] = "Your password has been changed successfully.";
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet, AllowAnonymous]
        public IActionResult ForgotPassword(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO dto, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                string resetPasswordUrl = Url.Action("ResetPassword", "Account", null, Request.Scheme)!;
                await _authService.SendPasswordResetEmailAsync(dto, resetPasswordUrl);
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(dto);
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpGet, AllowAnonymous]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                TempData["ErrorMessage"] = "Invalid password reset link.";
                return RedirectToAction("Login");
            }

            return View(new ResetPasswordVM(userId, token));
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                await _authService.ResetPasswordAsync(model);
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                return View(model);
            }

            TempData["SuccessMessage"] = "Your password has been changed successfully.";
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmailConfirmation(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                TempData["ErrorMessage"] = "Invalid user id.";
            }
            else
            {
                string confirmEmailUrl = Url.Action("ConfirmEmail", "Account", null, Request.Scheme)!;
                await _authService.SendEmailConfirmationAsync(userId, confirmEmailUrl);
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                await _authService.ConfirmEmailAsync(userId, token);
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            TempData["SuccessMessage"] = "Email successfully confirmed";
            return View();
        }

        #region Helper Methods

        private async Task SendEmailConfirmationAsync(string userId)
        {
            try
            {
                string confirmEmailUrl = Url.Action("ConfirmEmail", "Account", null, Request.Scheme)!;
                await _authService.SendEmailConfirmationAsync(userId, confirmEmailUrl);
            }
            catch (BusinessException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
        }

        #endregion
    }
}