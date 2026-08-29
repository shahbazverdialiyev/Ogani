using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.UsefulLinkDTO;
using Ogani.WebApp.Entities;

namespace Ogani.WebApp.Business.Services
{
    public class UsefulLinkManager : GenericManager<UsefulLink, UsefulLinkReadDTO, UsefulLinkReadDTO, UsefulLinkCreateDTO, UsefulLinkUpdateDTO>, IUsefulLinkService
    {
        public UsefulLinkManager(IUoW uoW, IMapper mapper, IValidator<UsefulLinkCreateDTO> createValidator, IValidator<UsefulLinkUpdateDTO> updateValidator)
            : base(uoW, mapper, createValidator, updateValidator) { }

        protected override async Task<List<ValidationFailure>> AddValidationFailureForCreateAsync(UsefulLinkCreateDTO usefulLinkDto)
        {
            bool isExist = await _uoW.UsefulLinkRepository.AnyAsync(u => u.Status &&
                                                                      u.Name == usefulLinkDto.Name);

            return isExist ? [new ValidationFailure(nameof(usefulLinkDto.Name), "Link with this name already exists.")] : [];
        }

        protected override async Task<List<ValidationFailure>> AddValidationFailureForUpdateAsync(UsefulLinkUpdateDTO usefulLinkDto)
        {
            bool isExist = await _uoW.UsefulLinkRepository.AnyAsync(u => u.Id != usefulLinkDto.Id &&
                                                                         u.Status &&
                                                                         u.Name == usefulLinkDto.Name);

            return isExist ? [new ValidationFailure(nameof(usefulLinkDto.Name), "Link with this name already exists.")] : [];
        }
    }
}
