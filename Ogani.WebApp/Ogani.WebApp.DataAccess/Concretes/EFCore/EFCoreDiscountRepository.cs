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
    public class EFCoreDiscountRepository : EFCoreRepository<Discount, int>, IDiscountRepository
    {
        public EFCoreDiscountRepository(OganiDbContext context) : base(context) { }

        public async Task<List<Discount>> GetActiveDiscountsAsync()
        {
            var now = DateTime.UtcNow;

            return await Table.Where(d => d.StartDate <= now && d.EndDate >= now)
                              .AsNoTracking()
                              .ToListAsync();
        }

        public async Task<List<Discount>> GetExpiredDiscountsAsync()
        {
            var now = DateTime.UtcNow;

            return await Table.Where(d => d.EndDate < now)
                              .AsNoTracking()
                              .ToListAsync();
        }

        public async Task<List<Discount>> GetUpcomingDiscountsAsync()
        {
            var now = DateTime.UtcNow;

            return await Table.Where(d => d.StartDate > now)
                              .AsNoTracking()
                              .ToListAsync();
        }

        public async Task<Discount?> GetByIdWithProductsAsync(int id, bool tracking = false)
        {
            IQueryable<Discount> query = Table
                .Include(x => x.Products)
                .Where(x => x.Id == id);

            return tracking
                ? await query.FirstOrDefaultAsync()
                : await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
