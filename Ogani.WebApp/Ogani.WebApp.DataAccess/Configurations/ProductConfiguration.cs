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
                   .HasMaxLength(200)
                   .IsUnicode();

            builder.Property(p => p.Description)
                   .HasMaxLength(1000);

            builder.Property(p => p.Price)
                   .HasPrecision(18, 2);

            builder.Property(p => p.Weight)
                   .HasPrecision(10, 2);

            builder.Property(p => p.ImageUrl)
                   .HasMaxLength(255);

            // For Nullable FK ON DELETE SET NULL
            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

}
