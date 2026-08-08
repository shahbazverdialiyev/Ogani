using Ogani.WebApp.DTOs.DiscountDTO;

namespace Ogani.WebApp.UI.Areas.Admin.ViewModels
{
    public class DiscountMultiSelectVM
    {
        public IReadOnlyCollection<DiscountReadDTO> Discounts { get; init; } = [];
        public ICollection<int> SelectedDiscountIds { get; init; } = [];
    }
}
