using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.UI.Areas.Admin.ViewModels;

namespace Ogani.WebApp.UI.Areas.Admin.ViewComponents
{
    public class DiscountProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public DiscountProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int discountId)
        {
            DiscountProductsVM model = new DiscountProductsVM()
            {
                Products = await _productService.GetProductsByDiscountIdAsync(discountId),
                DiscountId = discountId
            };

            return View(model);
        }
    }
}
