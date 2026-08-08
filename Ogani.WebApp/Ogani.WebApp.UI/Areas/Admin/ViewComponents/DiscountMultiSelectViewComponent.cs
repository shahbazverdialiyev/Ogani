using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.UI.Areas.Admin.ViewModels;

namespace Ogani.WebApp.UI.Areas.Admin.ViewComponents
{
    public class DiscountMultiSelectViewComponent : ViewComponent
    {
        private readonly IDiscountService _discountService;

        public DiscountMultiSelectViewComponent(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ICollection<int>? selectedDiscountIds)
        {
            DiscountMultiSelectVM model = new()
            {
                Discounts = await _discountService.GetAllAsync(),
                SelectedDiscountIds = selectedDiscountIds ?? []
            };
            return View(model);
        }
    }
}
