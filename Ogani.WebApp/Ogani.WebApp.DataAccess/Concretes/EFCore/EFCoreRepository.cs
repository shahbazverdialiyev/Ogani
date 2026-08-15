using Microsoft.EntityFrameworkCore;
using Ogani.WebApp.DataAccess.Contexts;
using Ogani.WebApp.DataAccess.Interfaces;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Concretes.EFCore
{
    public class EFCoreRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : notnull
    {
        protected readonly OganiDbContext _context;

        protected DbSet<TEntity> Table => _context.Set<TEntity>();

        public EFCoreRepository(OganiDbContext context)
        {
            _context = context;
        }

        public virtual async Task<TEntity?> GetByIdAsync(TKey id, bool tracking = false)
        {
            return tracking
                 ? await Table.FindAsync(id)
                 : await Table.AsNoTracking()
                              .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<TEntity?> GetForUpdateAsync(int id)
        {
            return await Table.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> GetAllAsync(bool tracking = false)
        {
            return tracking
                ? await Table.ToListAsync()
                : await Table.AsNoTracking().ToListAsync();
        }

        public async Task<List<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false)
        {
            IQueryable<TEntity> query = Table.Where(predicate);

            return tracking
                ? await query.ToListAsync()
                : await query.AsNoTracking().ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Table.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return predicate is null
                ? await Table.CountAsync()
                : await Table.CountAsync(predicate);
        }

        public async Task AddAsync(TEntity entity) => await Table.AddAsync(entity);

        public void Update(TEntity entity) => Table.Update(entity);

        public void Delete(TEntity entity) => Table.Remove(entity);
    }
}
