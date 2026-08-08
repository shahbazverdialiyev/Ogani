using AutoMapper;
using Ogani.WebApp.DTOs.DiscountDTO;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Mappings.AutoMapper
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Discount, DiscountReadDTO>();

            CreateMap<Discount, DiscountDetailReadDTO>()
                .ForMember(dest => dest.ProductNames, opt => opt.MapFrom(src => src.Products.Select(x => x.Name)));

            CreateMap<DiscountCreateDTO, Discount>()
            .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<DiscountUpdateDTO, Discount>()
                .ForMember(dest => dest.Products, opt => opt.Ignore());
        }
    }
}
