using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Abstracts
{
    public interface ICategoryRepository : IRepository<Category, int>
    {
        public Task<List<Category>> GetCategoriesWithProductsAsync();
    }
}
