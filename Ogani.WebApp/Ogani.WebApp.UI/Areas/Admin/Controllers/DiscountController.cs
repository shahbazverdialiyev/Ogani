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
        public async Task<IActionResult> Create(DiscountCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _discountService.AddAsync(dto);
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

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                DiscountUpdateDTO dto = await _discountService.GetForUpdateAsync(id);
                return View(dto);
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction((nameof(Index)));
        }

        [HttpPost]
        public async Task<IActionResult> Update(DiscountUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _discountService.UpdateAsync(dto);
                TempData["NotifySuccess"] = "Updated discount";
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            catch(NotFoundException ex)
            {
                TempData["NotifyError"]=ex.Message;
                RedirectToAction((nameof(Index)));
            }
            catch(BusinessException ex)
            {
                ModelState.AddModelError("",ex.Message);
            }

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _discountService.DeleteAsync(id);
                TempData["NotifySuccess"] = "Successfully deleted";
            }
            catch(NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
