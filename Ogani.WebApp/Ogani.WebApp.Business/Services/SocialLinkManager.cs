using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.SocialLinkDTO;
using Ogani.WebApp.Entities;

namespace Ogani.WebApp.Business.Services
{
    public class SocialLinkManager : GenericManager<SocialLink, SocialLinkReadDTO, SocialLinkReadDTO, SocialLinkCreateDTO, SocialLinkUpdateDTO>, ISocialLinkService
    {
        public SocialLinkManager(IUoW uoW, IMapper mapper, IValidator<SocialLinkCreateDTO> createValidator, IValidator<SocialLinkUpdateDTO> updateValidator)
            : base(uoW, mapper, createValidator, updateValidator) { }

        protected override async Task<List<ValidationFailure>> AddValidationFailureForCreateAsync(SocialLinkCreateDTO socialLinkDto)
        {
            bool isExist = await _uoW.SocialLinkRepository.AnyAsync(s => s.Status &&
                                                                         s.Platform == socialLinkDto.Platform &&
                                                                         s.Url == socialLinkDto.Url);

            return isExist ? [new ValidationFailure(nameof(socialLinkDto.Platform), "This platform already exists.")] : [];
        }

        protected override async Task<List<ValidationFailure>> AddValidationFailureForUpdateAsync(SocialLinkUpdateDTO socialLinkDto)
        {
            bool isExist = await _uoW.SocialLinkRepository.AnyAsync(s => s.Id != socialLinkDto.Id &&
                                                                         s.Status &&
                                                                         s.Platform == socialLinkDto.Platform &&
                                                                         s.Url == socialLinkDto.Url);

            return isExist ? [new ValidationFailure(nameof(socialLinkDto.Platform), "This platform already exists.")] : [];
        }
    }
}
