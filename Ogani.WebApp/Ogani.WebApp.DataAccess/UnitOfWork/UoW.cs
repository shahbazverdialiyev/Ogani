using Ogani.WebApp.DataAccess.Interfaces;
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
        protected readonly OganiDbContext _context;

        public UoW(OganiDbContext context)
        {
            _context = context;
        }

        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>
            where TKey : notnull
            => new EFCoreRepository<TEntity, TKey>(_context);

        public IProductRepository ProductRepository => new EFCoreProductRepository(_context);

        public ICategoryRepository CategoryRepository => new EFCoreCategoryRepository(_context);

        public IHeroRepository HeroRepository => new EFCoreHeroRepository(_context);

        public IDiscountRepository DiscountRepository => new EFCoreDiscountRepository(_context);

        public IContactRepository ContactRepository => new EFCoreContactRepository(_context);

        public ISocialLinkRepository SocialLinkRepository => new EFCoreSocialLinkRepository(_context);

        public IUsefulLinkRepository UsefulLinkRepository => new EFCoreUsefulLinkRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
