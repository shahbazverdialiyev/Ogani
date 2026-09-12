using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.Client;
using Ogani.WebApp.DTOs.Client.ProductDTO;
using Ogani.WebApp.Entities;
using Ogani.WebApp.UI.Models.Home;
using Ogani.WebApp.UI.Models.Shop;

namespace Ogani.WebApp.UI.Controllers
{
    public class ShopController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public ShopController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<IActionResult> Index([FromQuery] ProductFilterDTO filter)
        {

            PagedResultDTO<ProductCardDTO> products = await _productService.GetShopProductsAsync(filter);

            ViewBag.Categories = await _categoryService.GetCategoriesForUIAsync();
            ViewBag.Filter = filter;


            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> ProductsRow([FromQuery] ProductFilterDTO filter)
        {
            if (filter.Page < 1)
                filter.Page = 1;

            ViewBag.Categories = await _categoryService.GetCategoriesForUIAsync();
            ViewBag.Filter = filter;

            var result = await _productService.GetShopProductsAsync(filter);

            return PartialView("_ProductsRowPartial", result);
        }

        [HttpGet]
        public async Task<IActionResult> Products([FromQuery] ProductFilterDTO filter)
        {
            if (filter.Page < 1)
                filter.Page = 1;

            var result = await _productService.GetShopProductsAsync(filter);

            return PartialView("_ProductListPartial", result);
        }

        public async Task<IActionResult> Details(int id) => View(await _productService.GetProductDetailAsync(id));
    }
}
