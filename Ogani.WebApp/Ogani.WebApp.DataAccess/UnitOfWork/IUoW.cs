using Ogani.WebApp.DataAccess.Interfaces;
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

        IProductRepository ProductRepository { get; }

        ICategoryRepository CategoryRepository { get; }

        IHeroRepository HeroRepository { get; }

        IDiscountRepository DiscountRepository { get; }

        IContactRepository ContactRepository { get; }

        ISocialLinkRepository SocialLinkRepository { get; }

        IUsefulLinkRepository UsefulLinkRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
