using Ogani.WebApp.DTOs.CategoryDTO;
using Ogani.WebApp.DTOs.Client.CategoryDTO;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface ICategoryService : IService<CategoryReadDTO, CategoryDetailReadDTO, CategoryCreateDTO, CategoryUpdateDTO>
    {
        Task<List<CategoryReadDTO>> GetCategoriesWithProductsAsync();
        Task<IReadOnlyCollection<CategoryCardDTO>> GetCategoriesForUIAsync();
    }
}
