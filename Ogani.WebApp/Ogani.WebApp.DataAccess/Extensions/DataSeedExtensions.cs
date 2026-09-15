using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ogani.WebApp.Entities.Identity;

namespace Ogani.WebApp.DataAccess.Extensions
{
    public static class DataSeedExtensions
    {
        public static async Task<IHost> SeedIdentityAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

            string[] roles = { "Admin", "Member" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AppRole { Name = role });
                }
            }

            return host;
        }
    }
}