using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.DiscountDTO;

namespace Ogani.WebApp.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => View(await _discountService.GetAllAsync());

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                return View(await _discountService.GetByIdAsync(id));
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
        public async Task<IActionResult> Create(DiscountCreateDTO dicountDto)
        {
            if (!ModelState.IsValid)
                return View(dicountDto);

            try
            {
                await _discountService.AddAsync(dicountDto);
                TempData["NotifySuccess"] = "Created new discount";
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(dicountDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                DiscountUpdateDTO discountDto = await _discountService.GetForUpdateAsync(id);
                return View(discountDto);
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction((nameof(Index)));
        }

        [HttpPost]
        public async Task<IActionResult> Update(DiscountUpdateDTO discountDto)
        {
            if (!ModelState.IsValid)
                return View(discountDto);

            try
            {
                await _discountService.UpdateAsync(discountDto);
                TempData["NotifySuccess"] = $"Discount \"{discountDto.Code}\" was updated successfully.";
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
                return RedirectToAction((nameof(Index)));
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(discountDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _discountService.DeleteAsync(id);
                TempData["NotifySuccess"] = "Discount deleted successfully.";
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ManageProducts(int discountId)
        {
            try
            {
                DiscountProductsDTO discountDto = await _discountService.GetProductsForManageAsync(discountId);

                return View(discountDto);
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ManageProducts(DiscountProductsDTO discountDto)
        {
            try
            {
                await _discountService.UpdateProductsAsync(discountDto.DiscountId, discountDto.SelectedProductIds);
                TempData["NotifySuccess"] = "Discount products updated successfully.";
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction(nameof(Detail), new { id = discountDto.DiscountId });
        }
    }
}
