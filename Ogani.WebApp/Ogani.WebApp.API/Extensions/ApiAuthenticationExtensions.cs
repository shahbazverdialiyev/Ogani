using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ogani.WebApp.DTOs.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ogani.WebApp.API.Extensions
{
    public static class ApiAuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt").Get<JwtOptions>()
                ?? throw new InvalidOperationException("Jwt configuration is missing.");

            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,

                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

                            NameClaimType = ClaimTypes.Name,
                            RoleClaimType = ClaimTypes.Role,

                            ClockSkew = TimeSpan.Zero
                        };
                });

            return services;
        }

        public static string GenerateToken(this JwtOptions jwtOptions,AppUserDTO user)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Name, user.UserName),
                new (ClaimTypes.GivenName, user.FirstName),
                new (ClaimTypes.Surname, user.LastName),
                new (ClaimTypes.Email, user.Email),
                new ("IsEmailConfirmed", user.IsEmailConfirmed.ToString())
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                Expires = DateTime.UtcNow.AddMinutes(jwtOptions.ExpireMinutes),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}