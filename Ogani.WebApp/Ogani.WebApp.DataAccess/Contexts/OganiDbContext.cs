using Microsoft.EntityFrameworkCore;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Contexts
{
    public class OganiDbContext : DbContext
    {
        public OganiDbContext(DbContextOptions<OganiDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<UsefulLink> UsefulLinks { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Product>()
                .Where(e => e.State == EntityState.Modified))
            {
                entry.Property(p => p.ModifiedDate).CurrentValue = DateTime.UtcNow;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
