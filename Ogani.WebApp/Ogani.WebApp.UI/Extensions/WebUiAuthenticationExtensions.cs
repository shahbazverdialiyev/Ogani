using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Ogani.WebApp.DTOs.Auth;
using System.Security.Claims;

namespace Ogani.WebApp.UI.Extensions
{
    public static class WebUiAuthenticationExtensions
    {
        public static IServiceCollection AddCookieAuthentication(
            this IServiceCollection services)
        {
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "Ogani.Auth.Cookie";
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Errors/403";

                    options.Cookie.HttpOnly = true;
                });

            return services;
        }

        public static async Task SignInUserAsync(this HttpContext httpContext, AppUserDTO userDto, bool isPersistent = false)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, userDto.Id),
                new (ClaimTypes.Name, userDto.UserName),
                new (ClaimTypes.GivenName, userDto.FirstName),
                new (ClaimTypes.Surname, userDto.LastName),
                new (ClaimTypes.Email, userDto.Email),
                new ("IsEmailConfirmed", userDto.IsEmailConfirmed.ToString())
            };

            foreach (var role in userDto.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                ExpiresUtc = isPersistent
                    ? DateTimeOffset.UtcNow.AddDays(7)
                    : DateTimeOffset.UtcNow.AddHours(2)
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authProperties
                );
        }

        public static async Task SignOutUserAsync(this HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}