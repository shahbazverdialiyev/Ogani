using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ogani.WebApp.DataAccess.Contexts;
using Ogani.WebApp.DataAccess.UnitOfWork;
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

            //Uow
            services.AddScoped<IUoW, UoW>();

            return services;
        }
    }
}
