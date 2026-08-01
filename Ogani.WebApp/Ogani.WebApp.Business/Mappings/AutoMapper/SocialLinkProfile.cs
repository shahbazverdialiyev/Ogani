using AutoMapper;
using Ogani.WebApp.DTOs.SocialLinkDTO;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Mappings.AutoMapper
{
    public class SocialLinkProfile:Profile
    {
        public SocialLinkProfile()
        {
            CreateMap<SocialLink, SocialLinkReadDTO>();
            CreateMap<SocialLinkCreateDTO, SocialLink>();
            CreateMap<SocialLinkUpdateDTO, SocialLink>();
            CreateMap<SocialLinkReadDTO, SocialLinkUpdateDTO>();
        }
    }
}
