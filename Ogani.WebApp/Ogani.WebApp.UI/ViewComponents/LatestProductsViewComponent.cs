using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;

namespace Ogani.WebApp.UI.ViewComponents
{
    public class LatestProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public LatestProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync() => View(await _productService.GetLatestProductsAsync());
    }
}
