using Ogani.WebApp.DTOs.CategoryDTO;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface ICategoryService : IService<CategoryReadDTO,CategoryDetailReadDTO, CategoryCreateDTO, CategoryUpdateDTO>
    {
        Task<List<CategoryReadDTO>> GetCategoriesWithProductsAsync();
    }
}
