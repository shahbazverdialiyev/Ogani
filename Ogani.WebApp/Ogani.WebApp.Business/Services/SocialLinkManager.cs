using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.ContactDTO;
using Ogani.WebApp.DTOs.SocialLinkDTO;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services
{
    public class SocialLinkManager : GenericManager<SocialLink, SocialLinkReadDTO, SocialLinkReadDTO, SocialLinkCreateDTO, SocialLinkUpdateDTO>, ISocialLinkService
    {
        public SocialLinkManager(IUoW uoW, IMapper mapper, IValidator<SocialLinkCreateDTO> createValidator, IValidator<SocialLinkUpdateDTO> updateValidator)
            : base(uoW, mapper, createValidator, updateValidator) { }

        public override async Task<int> AddAsync(SocialLinkCreateDTO socialLinkDto)
        {
            bool isExist = await _uoW.SocialLinkRepository.AnyAsync(s => s.Status &&
                                                                      s.Platform == socialLinkDto.Platform &&
                                                                      s.Url == socialLinkDto.Url);

            if (isExist)
                throw new BusinessValidationException([new ValidationFailure(nameof(socialLinkDto.Platform), "This platform already exists.")]);

            return await base.AddAsync(socialLinkDto);
        }

        public override async Task UpdateAsync(SocialLinkUpdateDTO socialLinkDto)
        {
            bool isExist = await _uoW.SocialLinkRepository.AnyAsync(s => s.Id != socialLinkDto.Id &&
                                                                     s.Status &&
                                                                     s.Platform == socialLinkDto.Platform &&
                                                                     s.Url == socialLinkDto.Url);

            if (isExist)
                throw new BusinessValidationException([new ValidationFailure(nameof(socialLinkDto.Platform), "This platform already exists.")]);

            await base.UpdateAsync(socialLinkDto);
        }
    }
}
