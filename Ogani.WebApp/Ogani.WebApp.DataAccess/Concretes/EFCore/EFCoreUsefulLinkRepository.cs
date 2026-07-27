using Microsoft.EntityFrameworkCore;
using Ogani.WebApp.DataAccess.Contexts;
using Ogani.WebApp.DataAccess.Interfaces;
using Ogani.WebApp.Entities;
using Ogani.WebApp.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Concretes.EFCore
{
    public class EFCoreUsefulLinkRepository : EFCoreRepository<UsefulLink, int>, IUsefulLinkRepository
    {
        public EFCoreUsefulLinkRepository(OganiDbContext context) : base(context) { }

        public async Task<List<UsefulLink>> GetBySectionAsync(UsefulLinkSection section)
        {
            return await Table.Where(u => u.Section == section)
                              .AsNoTracking()
                              .ToListAsync();
        }
    }
}
