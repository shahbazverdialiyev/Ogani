using AutoMapper;
using Ogani.WebApp.DTOs.ContactDTO;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Mappings.AutoMapper
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<Contact,ContactReadDTO>();
            CreateMap<ContactCreateDTO, Contact>();
            CreateMap<ContactUpdateDTO, Contact>();
            CreateMap<ContactReadDTO,ContactUpdateDTO>();
        }
    }
}
