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
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(c => c.Title)
                .IsUnique()
                .HasFilter("[Status] = 1");

            builder.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.Icon)
                .HasMaxLength(100);
        }
    }
}
