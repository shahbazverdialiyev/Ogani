using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Configurations
{
    public class HeroConfiguration : IEntityTypeConfiguration<Hero>
    {
        public void Configure(EntityTypeBuilder<Hero> builder)
        {
            builder.Property(h => h.Title)
           .IsRequired()
           .HasMaxLength(100);

            builder.Property(h => h.Subtitle)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(h => h.ButtonText)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(h => h.ButtonUrl)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(h => h.ImageUrl)
                .IsRequired();

            builder.Property(h => h.IsActive)
                .HasDefaultValue(false);

            // Only one active hero (DB constraint)
            builder.HasIndex(h => h.IsActive)
                .IsUnique()
                .HasFilter("[IsActive] = 1");
        }
    }
}
