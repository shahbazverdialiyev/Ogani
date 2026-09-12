using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;

namespace Ogani.WebApp.UI.ViewComponents
{
    public class DiscountedProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public DiscountedProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync() => View(await _productService.GetDiscountedProductsAsync());
    }
}
