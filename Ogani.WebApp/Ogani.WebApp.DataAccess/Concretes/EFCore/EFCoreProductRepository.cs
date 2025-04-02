using Microsoft.EntityFrameworkCore;
using Ogani.WebApp.DataAccess.Abstracts;
using Ogani.WebApp.DataAccess.Contexts;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Concretes.EFCore
{
    public class EFCoreProductRepository:EFCoreRepository<Product, int>, IProductRepository
    {
        public EFCoreProductRepository(OganiDbContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)=>await _context.Products.Where(x => x.CategoryId == categoryId).ToListAsync();

    }
}
