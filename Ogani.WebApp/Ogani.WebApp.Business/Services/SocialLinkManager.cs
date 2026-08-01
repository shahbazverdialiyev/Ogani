using AutoMapper;
using FluentValidation;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.SocialLinkDTO;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services
{
    public class SocialLinkManager : GenericManager<SocialLink, SocialLinkReadDTO, SocialLinkCreateDTO, SocialLinkUpdateDTO>, ISocialLinkService
    {
        public SocialLinkManager(IUoW uoW, IMapper mapper, IValidator<SocialLinkCreateDTO> createValidator, IValidator<SocialLinkUpdateDTO> updateValidator)
            : base(uoW, mapper, createValidator, updateValidator) { }
    }
}
