using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.CategoryDTO;
using Ogani.WebApp.Entities;

namespace Ogani.WebApp.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => View(await _categoryService.GetAllAsync());

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                return View(await _categoryService.GetByIdAsync(id));
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
        public async Task<IActionResult> Create(CategoryCreateDTO categoryDto)
        {
            if (!ModelState.IsValid)
                return View(categoryDto);

            try
            {
                await _categoryService.AddAsync(categoryDto);
                TempData["NotifySuccess"] = $"Product \"{categoryDto.Name}\" was created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(categoryDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                CategoryUpdateDTO categoryDto = await _categoryService.GetForUpdateAsync(id);
                return View(categoryDto);
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateDTO categoryDto)
        {
            if (!ModelState.IsValid)
                return View(categoryDto);

            try
            {
                await _categoryService.UpdateAsync(categoryDto);
                TempData["NotifySuccess"] = $"Category \"{categoryDto.Name}\" was updated successfully.";

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

            return View(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                TempData["NotifySuccess"] = "Category deleted successfully.";
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
