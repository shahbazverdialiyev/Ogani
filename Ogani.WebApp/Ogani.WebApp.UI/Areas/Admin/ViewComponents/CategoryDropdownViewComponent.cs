using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.UI.Areas.Admin.ViewModels;

namespace Ogani.WebApp.UI.Areas.Admin.ViewComponents
{
    public class CategoryDropdownViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoryDropdownViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? selectedCategoryId)
        {
            CategoryDropdownVM model = new()
            {
                Categories = await _categoryService.GetAllAsync(),
                SelectedCategoryId = selectedCategoryId
            };

            return View(model);
        }
    }
}
