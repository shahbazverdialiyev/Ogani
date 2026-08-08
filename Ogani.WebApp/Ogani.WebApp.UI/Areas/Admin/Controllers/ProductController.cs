using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.ProductDTO;

namespace Ogani.WebApp.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, ICategoryService categoryService, IDiscountService discountService, IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _discountService = discountService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index() => View(await _productService.GetAllAsync());

        [HttpGet]
        public IActionResult Create() => View(new ProductCreateDTO());

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDTO product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            try
            {
                await _productService.AddAsync(product);
                TempData["NotifySuccess"] = "Created new Product";

                return RedirectToAction(nameof(Index));
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            catch(BusinessException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                ProductUpdateDTO updateDto = await _productService.GetForUpdateAsync(id);

                return View(updateDto);
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateDTO updateDto)
        {
            if (!ModelState.IsValid)
                return View(updateDto);

            try
            {
                await _productService.UpdateAsync(updateDto);
                TempData["NotifySuccess"] = "Updated Product";

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

            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteAsync(id);
                TempData["NotifySuccess"] = "Deleted Product";

                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
