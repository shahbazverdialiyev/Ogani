using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.UI.Models.Home;

namespace Ogani.WebApp.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public HomeController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM model = new()
            {
                Categories = await _categoryService.GetCategoriesForUIAsync(),
                FeaturedProducts = await _productService.GetFeaturedProductsAsync(),
                LatestProducts = await _productService.GetLatestProductsAsync()
            };

            return View(model);
        }
    }
}
