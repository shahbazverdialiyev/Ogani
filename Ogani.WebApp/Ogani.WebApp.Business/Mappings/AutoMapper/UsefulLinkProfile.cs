using AutoMapper;
using Ogani.WebApp.DTOs.UsefulLinkDTO;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Mappings.AutoMapper
{
    public class UsefulLinkProfile:Profile
    {
        public UsefulLinkProfile()
        {
            CreateMap<UsefulLink, UsefulLinkReadDTO>();
            CreateMap<UsefulLinkCreateDTO, UsefulLink>();
            CreateMap<UsefulLinkUpdateDTO, UsefulLink>();
            CreateMap<UsefulLink, UsefulLinkUpdateDTO>();
        }
    }
}
