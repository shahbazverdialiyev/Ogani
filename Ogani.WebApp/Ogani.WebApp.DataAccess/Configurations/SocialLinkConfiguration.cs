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
    public class SocialLinkConfiguration : IEntityTypeConfiguration<SocialLink>
    {
        public void Configure(EntityTypeBuilder<SocialLink> builder)
        {
            builder.Property(s => s.Platform)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(s => new { s.Platform, s.Url })
                .IsUnique()
                .HasFilter("[Status] = 1");

            builder.Property(s => s.Url)
                .IsRequired()
                .HasMaxLength(1000);
        }
    }
}
