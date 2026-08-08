using AutoMapper;
using Ogani.WebApp.Entities;
using Ogani.WebApp.DTOs.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Mappings.AutoMapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductReadDTO>();

            CreateMap<Product, ProductDetailReadDTO>();

            CreateMap<ProductCreateDTO, Product>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Discounts, opt => opt.Ignore());

            CreateMap<Product, ProductUpdateDTO>();

            CreateMap<ProductUpdateDTO, Product>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Discounts, opt => opt.Ignore());
        }
    }
}
