using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.DTOs.Auth;
using Ogani.WebApp.Business.Services;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.Auth;

namespace Ogani.WebApp.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditRoles(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                var dto = new UpdateUserRolesDTO
                {
                    UserId = user.Id,
                    Roles = user.Roles
                };

                ViewBag.UserEmail = user.Email;
                ViewBag.FullName = user.FullName;

                return View(dto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(UpdateUserRolesDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _userService.UpdateUserRolesAsync(dto);
                TempData["SuccessMessage"] = "User roles updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                TempData["SuccessMessage"] = "User deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}