using Ogani.WebApp.DataAccess.Abstracts;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.UnitOfWork
{
    public interface IUoW
    {
        IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>
            where TKey : notnull;


        Task SaveChangesAsync();
    }
}
