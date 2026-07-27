using Microsoft.EntityFrameworkCore;
using Ogani.WebApp.DataAccess.Contexts;
using Ogani.WebApp.DataAccess.Interfaces;
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
        public async Task<List<Category>> GetCategoriesWithProductsAsync(bool tracking = false)
        {
            IQueryable<Category> query = Table.Include(c => c.Products);

            return tracking
                ? await query.ToListAsync()
                : await query.AsNoTracking().ToListAsync();
        }

        public Task<Category?> GetCategoryWithProductsAsync(int id)
        {
            return Table.Include(c => c.Products)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
