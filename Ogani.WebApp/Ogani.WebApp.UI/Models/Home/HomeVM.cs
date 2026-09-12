using Ogani.WebApp.DTOs.Client.CategoryDTO;
using Ogani.WebApp.DTOs.Client.ProductDTO;

namespace Ogani.WebApp.UI.Models.Home
{
    public class HomeVM
    {
        public IReadOnlyCollection<CategoryCardDTO> Categories { get; set; } = [];
        public IReadOnlyCollection<ProductCardDTO> FeaturedProducts { get; set; } = [];
        public IReadOnlyCollection<ProductCardDTO> LatestProducts { get; set; } = [];
    }
}
