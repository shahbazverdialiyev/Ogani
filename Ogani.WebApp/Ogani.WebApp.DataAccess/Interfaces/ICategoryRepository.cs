using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Interfaces
{
    public interface ICategoryRepository : IRepository<Category, int>
    {
        Task<List<Category>> GetCategoriesWithProductsAsync(bool tracking = false);

        Task<Category?> GetCategoryWithProductsAsync(int id);
    }
}
