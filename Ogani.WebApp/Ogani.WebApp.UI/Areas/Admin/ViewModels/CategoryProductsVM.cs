using Ogani.WebApp.DTOs.ProductDTO;

namespace Ogani.WebApp.UI.Areas.Admin.ViewModels
{
    public class CategoryProductsVM
    {
        public IReadOnlyCollection<ProductReadDTO> Products { get; set; } = [];
        public int? CategoryId { get; set; }
    }
}
