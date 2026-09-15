using Microsoft.AspNetCore.Authentication.Cookies;

namespace Ogani.WebApp.UI.Extensions
{
    public static class MvcAuthenticationExtensions
    {
        public static IServiceCollection AddMvcAuthentication(
            this IServiceCollection services)
        {
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Errors/403";
                });

            return services;
        }
    }
}