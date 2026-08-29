using Ogani.WebApp.DTOs.DiscountDTO;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IDiscountService : IService<DiscountReadDTO, DiscountDetailReadDTO, DiscountCreateDTO, DiscountUpdateDTO>
    {
        Task<DiscountProductsDTO> GetProductsForManageAsync(int discountId);

        Task UpdateProductsAsync(int discountId,ICollection<int> ProductIds);
    }
}
