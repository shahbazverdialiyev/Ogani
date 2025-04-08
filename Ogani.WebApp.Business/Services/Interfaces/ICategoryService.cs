using Ogani.WebApp.DTOs.CategoryDTO;
using Ogani.WebApp.DTOs.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface ICategoryService : IService<CategoryReadDTO, CategoryCreateDTO, CategoryUpdateDTO>
    {
        Task<List<CategoryReadDTO>> GetCategoriesWithProductsAsync();
    }
}
