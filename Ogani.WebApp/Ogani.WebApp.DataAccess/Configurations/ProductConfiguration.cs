using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex(p => p.Name)
                   .IsUnique();

            builder.Property(p => p.Description)
                   .HasMaxLength(1000);

            builder.Property(p => p.Info)
                   .HasMaxLength(500);

            builder.Property(p => p.Price)
                   .HasPrecision(18, 2);

            builder.Property(p => p.Weight)
                   .HasPrecision(10, 2);

            builder.Property(p => p.ImageUrl)
                   .HasMaxLength(1000);

            // For Nullable FK ON DELETE SET NULL
            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

}
