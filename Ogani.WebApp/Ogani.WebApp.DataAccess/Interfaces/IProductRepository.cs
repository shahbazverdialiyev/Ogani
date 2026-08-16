using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Interfaces
{
    public interface IProductRepository : IRepository<Product, int>
    {
        Task<List<Product>> GetAvailableProductsAsync();

        Task<List<Product>> GetFeaturedProductsAsync();

        Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId, bool tracking = false);

        Task<List<Product>> GetProductsByDiscountIdAsync(int categoryId, bool tracking = false);

        Task<List<Product>> GetProductsWithCategoryAsync();

        Task<Product?> GetProductDetailsAsync(int id);

        Task<List<Product>> SearchAsync(string keyword);
    }
}
