using Ogani.WebApp.DataAccess.Abstracts;
using Ogani.WebApp.DataAccess.Concretes.EFCore;
using Ogani.WebApp.DataAccess.Contexts;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.UnitOfWork
{
    public class UoW : IUoW
    {
        private readonly OganiDbContext _context;

        public UoW(OganiDbContext context)
        {
            _context = context;
        }
        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>
            where TKey : notnull
        {
            return new EFCoreRepository<TEntity, TKey>(_context);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
