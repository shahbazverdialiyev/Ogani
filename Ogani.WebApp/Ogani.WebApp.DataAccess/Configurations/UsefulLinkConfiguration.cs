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
    public class UsefulLinkConfiguration : IEntityTypeConfiguration<UsefulLink>
    {
        public void Configure(EntityTypeBuilder<UsefulLink> builder)
        {
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(u => u.Name )
                .IsUnique()
                .HasFilter("[Status] = 1");

            builder.Property(u => u.Url)
                .IsRequired()
                .HasMaxLength(1000);
        }
    }
}
