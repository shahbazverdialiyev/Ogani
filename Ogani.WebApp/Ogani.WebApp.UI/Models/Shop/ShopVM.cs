using Ogani.WebApp.DTOs.Client;
using Ogani.WebApp.DTOs.Client.CategoryDTO;
using Ogani.WebApp.DTOs.Client.ProductDTO;

namespace Ogani.WebApp.UI.Models.Shop
{
    public class ShopVM
    {
        public PagedResultDTO<ProductCardDTO>? Products { get; set; } = null!;
        public ProductFilterDTO Filter { get; set; } = new();
        public IReadOnlyCollection<CategoryCardDTO> Categories { get; set; } = [];
    }
}
