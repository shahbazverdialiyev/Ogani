using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.SocialLinkDTO;

namespace Ogani.WebApp.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SocialLinkController : Controller
    {
        private readonly ISocialLinkService _socialLinkService;

        public SocialLinkController(ISocialLinkService socialLinkService)
        {
            _socialLinkService = socialLinkService;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => View(await _socialLinkService.GetAllAsync());

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                return View(await _socialLinkService.GetByIdAsync(id));
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SocialLinkCreateDTO linkDto)
        {
            if (!ModelState.IsValid)
                return View(linkDto);

            try
            {
                await _socialLinkService.AddAsync(linkDto);
                TempData["NotifySuccess"] = $"Social Link \"{linkDto.Platform}\" was created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(linkDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                return View(await _socialLinkService.GetForUpdateAsync(id));
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(SocialLinkUpdateDTO linkDto)
        {
            if (!ModelState.IsValid)
                return View(linkDto);

            try
            {
                await _socialLinkService.UpdateAsync(linkDto);
                TempData["NotifySuccess"] = $"Social Link \"{linkDto.Platform}\" was updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;

                return RedirectToAction(nameof(Index));
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(linkDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _socialLinkService.DeleteAsync(id);
                TempData["NotifySuccess"] = "Social Link item deleted successfully.";
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
