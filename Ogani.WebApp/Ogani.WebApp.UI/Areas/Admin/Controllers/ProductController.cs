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
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId)
        {
            var products = categoryId.HasValue
                ? await _productService.GetProductsByCategoryIdAsync(categoryId.Value)
                : await _productService.GetAllAsync();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                return View(await _productService.GetByIdAsync(id));
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create() => View(new ProductCreateDTO());

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            try
            {
                int productId = await _productService.AddAsync(productDto);
                TempData["NotifySuccess"] = $"Product \"{productDto.Name}\" was created successfully.";

                return RedirectToAction(nameof(Detail), new { id = productId });
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

            return View(productDto);
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
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateDTO updateDto)
        {
            if (!ModelState.IsValid)
                return View(updateDto);

            try
            {
                await _productService.UpdateAsync(updateDto);
                TempData["NotifySuccess"] = $"Product \"{updateDto.Name}\" was updated successfully.";

                return RedirectToAction(nameof(Detail), new { id = updateDto.Id });
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
                TempData["NotifySuccess"] = "Product deleted successfully.";
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;

            }

            return RedirectToAction(nameof(Index));
        }
    }
}
