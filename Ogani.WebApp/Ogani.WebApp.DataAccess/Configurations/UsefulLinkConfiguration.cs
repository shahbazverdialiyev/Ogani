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
    internal class UsefulLinkConfiguration : IEntityTypeConfiguration<UsefulLink>
    {
        public void Configure(EntityTypeBuilder<UsefulLink> builder)
        {
            builder.Property(u=>u.Name)
                .IsRequired();

            builder.Property(u => u.Url)
                .IsRequired();
        }
    }
}
