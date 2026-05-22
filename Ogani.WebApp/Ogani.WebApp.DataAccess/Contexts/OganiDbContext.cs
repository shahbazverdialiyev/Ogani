using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Contexts
{
    public class OganiDbContext: DbContext
    {
        public OganiDbContext(DbContextOptions<OganiDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Entities.Product> Products { get; set; }
        public DbSet<Entities.Category> Categories { get; set; }
        public DbSet<Entities.Hero> Heroes { get; set; }
        public DbSet<Entities.Discount> Discounts { get; set; }
        public DbSet<Entities.SocialLink> SocialLinks { get; set; }
        public DbSet<Entities.UsefulLink> UsefulLinks { get; set; }
        public DbSet<Entities.Contact> Contacts { get; set; }
    }
}
