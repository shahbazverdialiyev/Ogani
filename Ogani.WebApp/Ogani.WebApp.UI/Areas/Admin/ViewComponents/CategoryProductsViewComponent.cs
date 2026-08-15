using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.DTOs.ProductDTO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.UI.Areas.Admin.ViewModels;

namespace Ogani.WebApp.ViewComponents
{
    public class CategoryProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public CategoryProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            CategoryProductsVM model = new CategoryProductsVM()
            {
                Products = await _productService.GetProductsByCategoryIdAsync(categoryId),
                CategoryId = categoryId
            };
            return View(model);
        }
    }
}