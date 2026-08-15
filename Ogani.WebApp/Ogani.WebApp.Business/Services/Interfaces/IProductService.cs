using Ogani.WebApp.DTOs.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services.Interfaces
{
    public interface IProductService : IService<ProductReadDTO, ProductDetailReadDTO, ProductCreateDTO, ProductUpdateDTO>
    {
        Task<IReadOnlyCollection<ProductReadDTO>> GetProductsByCategoryIdAsync(int categoryId);
    }
}
