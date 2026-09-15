using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.UsefulLinkDTO;

namespace Ogani.WebApp.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsefulLinkController : Controller
    {
        private readonly IUsefulLinkService _usefulLinkService;

        public UsefulLinkController(IUsefulLinkService usefulLinkService)
        {
            _usefulLinkService = usefulLinkService;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => View(await _usefulLinkService.GetAllAsync());

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                return View(await _usefulLinkService.GetByIdAsync(id));
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
        public async Task<IActionResult> Create(UsefulLinkCreateDTO linkDto)
        {
            if (!ModelState.IsValid)
                return View(linkDto);

            try
            {
                await _usefulLinkService.AddAsync(linkDto);
                TempData["NotifySuccess"] = $"Useful link \"{linkDto.Name}\" was created successfully.";

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
                return View(await _usefulLinkService.GetForUpdateAsync(id));
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UsefulLinkUpdateDTO linkDto)
        {
            if (!ModelState.IsValid)
                return View(linkDto);

            try
            {
                await _usefulLinkService.UpdateAsync(linkDto);
                TempData["NotifySuccess"] = $"Useful link \"{linkDto.Name}\" was updated successfully.";

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
                await _usefulLinkService.DeleteAsync(id);
                TempData["NotifySuccess"] = "Useful link item deleted successfully.";
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
