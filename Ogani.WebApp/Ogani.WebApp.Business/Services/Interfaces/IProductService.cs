using Ogani.WebApp.DTOs.Client;
using Ogani.WebApp.DTOs.Client.ProductDTO;
using Ogani.WebApp.DTOs.ProductDTO;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IProductService : IService<ProductReadDTO, ProductDetailReadDTO, ProductCreateDTO, ProductUpdateDTO>
    {
        Task<IReadOnlyCollection<ProductReadDTO>> GetProductsByCategoryIdAsync(int categoryId);
        Task<IReadOnlyCollection<ProductReadDTO>> GetProductsByDiscountIdAsync(int categoryId);

        Task<PagedResultDTO<ProductCardDTO>> GetProductsForShopAsync(ProductFilterDTO filter);
        Task<IReadOnlyCollection<ProductCardDTO>> GetFeaturedProductsAsync();
        Task<IReadOnlyCollection<ProductCardDTO>> GetLatestProductsAsync();
        Task<ProductDetailDTO> GetProductDetailAsync(int id);
        Task<IReadOnlyCollection<ProductCardDTO>> GetProductsByCategoryForUIAsync(int id);
        Task<IReadOnlyCollection<ProductCardDTO>> GetDiscountedProductsAsync();
    }
}
