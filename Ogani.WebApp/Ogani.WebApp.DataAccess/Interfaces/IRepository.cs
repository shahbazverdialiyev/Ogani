using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Interfaces
{
    public interface IRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : notnull
    {
        Task<List<TEntity>> GetAllAsync(bool tracking = false);

        Task<TEntity?> GetByIdAsync(TKey id, bool tracking = false);

        Task<List<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
