using Ogani.WebApp.DTOs.CategoryDTO;

namespace Ogani.WebApp.UI.Areas.Admin.ViewModels
{
    public class CategoryDropdownVM
    {
        public IReadOnlyCollection<CategoryReadDTO> Categories { get; init; } = [];
        public int? SelectedCategoryId { get; init; }
    }
}
