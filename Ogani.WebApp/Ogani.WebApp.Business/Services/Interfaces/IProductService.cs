using Ogani.WebApp.DTOs.ProductDTO;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IProductService : IService<ProductReadDTO, ProductDetailReadDTO, ProductCreateDTO, ProductUpdateDTO>
    {
        Task<IReadOnlyCollection<ProductReadDTO>> GetProductsByCategoryIdAsync(int categoryId);

        Task<IReadOnlyCollection<ProductReadDTO>> GetProductsByDiscountIdAsync(int categoryId);
    }
}
