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
    public class EFCoreRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : notnull
    {
        public readonly OganiDbContext _context;

        public EFCoreRepository(OganiDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity> GetByIdAsync(TKey id) => await _context.Set<TEntity>().FindAsync(id);
        public async Task<List<TEntity>> GetAllAsync() => await _context.Set<TEntity>().ToListAsync();
        public async Task AddAsync(TEntity entity) => await _context.Set<TEntity>().AddAsync(entity);
        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);
        public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);
    }
}
