using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ogani.WebApp.DataAccess.Contexts;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.DataAccess.Extensions
{
    public static class DataAccessExtension
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, string? connectionString)
        {
            // DbContext
            services.AddDbContext<OganiDbContext>(options =>
            options.UseSqlServer(connectionString));

            //Identity
            services.AddIdentityCore<AppUser>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 30;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<OganiDbContext>()
            .AddDefaultTokenProviders();


            //Uow
            services.AddScoped<IUoW, UoW>();

            return services;
        }
    }
}
