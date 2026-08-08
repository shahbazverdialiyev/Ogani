using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Ogani.WebApp.Business.Mappings.AutoMapper;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.Business.Services;
using Ogani.WebApp.Business.Validators.ProductValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ogani.WebApp.Business.Services.Storage;

namespace Ogani.WebApp.Business.Extensions
{
    public static class BusinessExtension
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            //AutoMapper
            services.AddAutoMapper(typeof(ProductProfile).Assembly);

            //FluentValidation
            services.AddValidatorsFromAssemblyContaining<ProductCreateValidator>();

            //Managers
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IDiscountService, DiscountManager>();
            services.AddScoped<IContactService, ContactManager>();
            services.AddScoped<ISocialLinkService, SocialLinkManager>();

            //FileService
            services.AddScoped<IFileService, LocalFileService>();

            return services;
        }
    }
}
