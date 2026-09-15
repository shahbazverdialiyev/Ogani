using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.HeroDTO;

namespace Ogani.WebApp.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HeroController : Controller
    {
        private readonly IHeroService _heroService;

        public HeroController(IHeroService heroService)
        {
            _heroService = heroService;
        }

        public async Task<IActionResult> Index() => View(await _heroService.GetAllAsync());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActive(int id)
        {
            try
            {
                await _heroService.SetHeroActiveAsync(id);
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
        public async Task<IActionResult> Create(HeroCreateDTO heroDto)
        {
            if (!ModelState.IsValid)
                return View(heroDto);

            try
            {
                await _heroService.AddAsync(heroDto);
                TempData["NotifySuccess"] = $"Hero \"{heroDto.Title}\" was created successfully.";

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

            return View(heroDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                return View(await _heroService.GetForUpdateAsync(id));
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(HeroUpdateDTO heroDto)
        {
            if (!ModelState.IsValid)
                return View(heroDto);

            try
            {
                await _heroService.UpdateAsync(heroDto);
                TempData["NotifySuccess"] = $"Hero \"{heroDto.Title}\" was updated successfully.";

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

            return View(heroDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _heroService.DeleteAsync(id);
                TempData["NotifySuccess"]= "Hero deleted successfully.";
            }
            catch(NotFoundException ex)
            {
                TempData["NotifyError"]= ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
