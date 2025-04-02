using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Abstracts
{
    public interface IProductRepository:IRepository<Product,int>
    {
        public Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
    }
}
