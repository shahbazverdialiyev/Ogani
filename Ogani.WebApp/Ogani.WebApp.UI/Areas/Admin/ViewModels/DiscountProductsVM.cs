using Ogani.WebApp.DTOs.ProductDTO;

namespace Ogani.WebApp.UI.Areas.Admin.ViewModels
{
    public class DiscountProductsVM
    {
        public IReadOnlyCollection<ProductReadDTO> Products { get; set; } = [];
        public int? DiscountId { get; set; }
    }
}
