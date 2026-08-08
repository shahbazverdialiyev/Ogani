using AutoMapper;
using Ogani.WebApp.Entities;
using Ogani.WebApp.DTOs.CategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ogani.WebApp.DTOs.ProductDTO;

namespace Ogani.WebApp.Business.Mappings.AutoMapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryReadDTO>();

            CreateMap<Category, CategoryDetailReadDTO>()
                .ForMember(dest => dest.ProductNames, opt => opt.MapFrom(src => src.Products.Select(x => x.Name)));

            CreateMap<CategoryCreateDTO, Category>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<CategoryUpdateDTO, Category>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

        }
    }
}
