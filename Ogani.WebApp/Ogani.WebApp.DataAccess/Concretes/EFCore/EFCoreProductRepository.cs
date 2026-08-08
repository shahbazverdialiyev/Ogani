using Microsoft.EntityFrameworkCore;
using Ogani.WebApp.DataAccess.Contexts;
using Ogani.WebApp.DataAccess.Interfaces;
using Ogani.WebApp.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Concretes.EFCore
{
    public class EFCoreProductRepository : EFCoreRepository<Product, int>, IProductRepository
    {
        public EFCoreProductRepository(OganiDbContext context) : base(context) { }

        public override async Task<List<Product>> GetAllAsync(bool tracking = false)
        {
            IQueryable<Product> query = Table.Include(p => p.Category);

            return tracking
                ? await query.ToListAsync()
                : await query.AsNoTracking().ToListAsync();
        }

        public override async Task<Product?> GetForUpdateAsync(int id)
        {
            return await Table.Include(p => p.Category)
                              .Include(p => p.Discounts)
                              .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Product>> GetAvailableProductsAsync()
        {
            return await Table.Where(p => p.IsAvailable)
                              .AsNoTracking()
                              .ToListAsync();
        }

        public async Task<List<Product>> GetFeaturedProductsAsync()
        {
            return await Table.Where(p => p.IsFeatured)
                              .AsNoTracking()
                              .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId, bool tracking = false)
        {
            IQueryable<Product> query = Table.Where(p => p.CategoryId == categoryId);

            return tracking
                ? await query.ToListAsync()
                : await query.AsNoTracking()
                             .ToListAsync();
        }

        public async Task<List<Product>> GetProductsWithCategoryAsync()
        {
            return await Table.Include(p => p.Category)
                              .AsNoTracking()
                              .ToListAsync();
        }

        public async Task<Product?> GetProductDetailsAsync(int id)
        {
            return await Table.Include(p => p.Category)
                              .Include(p => p.Discounts)
                              .AsNoTracking()
                              .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Product>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<Product>();

            keyword = keyword.Trim();

            return await Table.Where(p => EF.Functions.Like(p.Name, $"%{keyword}%"))
                              .AsNoTracking()
                              .ToListAsync();
        }
    }
}
