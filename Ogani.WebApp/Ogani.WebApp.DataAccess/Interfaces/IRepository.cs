using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Abstracts
{
    public interface IRepository<TEntity,TKey> 
        where TEntity : BaseEntity<TKey> 
        where TKey :notnull
    {
        public Task<TEntity> GetByIdAsync(TKey id);
        public Task<List<TEntity>> GetAllAsync();
        public Task AddAsync(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);
    }
}
