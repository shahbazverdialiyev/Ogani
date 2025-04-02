using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100)
                   .IsUnicode();

            builder.Property(c => c.Description)
                   .HasMaxLength(500);

            builder.Property(c => c.ImageUrl)
                   .HasMaxLength(255);
        }
    }

}
