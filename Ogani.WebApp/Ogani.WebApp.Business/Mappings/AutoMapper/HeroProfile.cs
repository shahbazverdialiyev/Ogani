using AutoMapper;
using Ogani.WebApp.DTOs.HeroDTO;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Mappings.AutoMapper
{
    public class HeroProfile : Profile
    {
        public HeroProfile()
        {
            CreateMap<Hero, HeroReadDTO>();
            CreateMap<Hero, HeroDetailReadDTO>();

            CreateMap<HeroCreateDTO, Hero>()
                .ForMember(x => x.ImageUrl, opt => opt.Ignore());

            CreateMap<Hero, HeroUpdateDTO>();

            CreateMap<HeroUpdateDTO, Hero>()
                .ForMember(x => x.ImageUrl, opt => opt.Ignore());
        }
    }
}