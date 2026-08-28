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
    public class EFCoreHeroRepository : EFCoreRepository<Hero, int>, IHeroRepository
    {
        public EFCoreHeroRepository(OganiDbContext context) : base(context) { }

        public async Task<Hero?> GetActiveHeroAsync(bool tracking = false)
        {
            IQueryable<Hero> query = Table.Where(h => h.IsActive);

            return tracking
                ? await query.FirstOrDefaultAsync()
                : await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
