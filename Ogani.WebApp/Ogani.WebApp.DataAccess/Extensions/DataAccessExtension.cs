using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ogani.WebApp.DataAccess.Contexts;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.Entities.Identity;

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
            services.AddIdentity();

            //Uow
            services.AddScoped<IUoW, UoW>();

            return services;
        }

        private static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequiredUniqueChars = 2;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<OganiDbContext>()
            .AddUserManager<UserManager<AppUser>>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
