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

            CreateMap<Discount, DiscountDetailReadDTO>();

            CreateMap<DiscountCreateDTO, Discount>()
            .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<Discount, DiscountUpdateDTO>();

            CreateMap<DiscountUpdateDTO, Discount>()
                .ForMember(dest => dest.Code, p => p.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore());
        }
    }
}
