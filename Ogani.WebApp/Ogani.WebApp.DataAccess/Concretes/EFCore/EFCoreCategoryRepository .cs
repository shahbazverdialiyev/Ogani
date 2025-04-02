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
    public class EFCoreCategoryRepository : EFCoreRepository<Category, int>, ICategoryRepository
    {
        public EFCoreCategoryRepository(OganiDbContext context) : base(context)
        {
        }
        public async Task<List<Category>> GetCategoriesWithProductsAsync() => await _context.Categories.Include(x => x.Products).ToListAsync();
    }
}
