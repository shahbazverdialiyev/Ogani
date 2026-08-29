using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.SocialLinkDTO;
using Ogani.WebApp.DTOs.UsefulLinkDTO;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services
{
    public class UsefulLinkManager : GenericManager<UsefulLink, UsefulLinkReadDTO, UsefulLinkReadDTO, UsefulLinkCreateDTO, UsefulLinkUpdateDTO>, IUsefulLinkService
    {
        public UsefulLinkManager(IUoW uoW, IMapper mapper, IValidator<UsefulLinkCreateDTO> createValidator, IValidator<UsefulLinkUpdateDTO> updateValidator)
            : base(uoW, mapper, createValidator, updateValidator) { }

        public override async Task<int> AddAsync(UsefulLinkCreateDTO usefulLinkDto)
        {
            bool isExist = await _uoW.UsefulLinkRepository.AnyAsync(u => u.Status &&
                                                                      u.Name == usefulLinkDto.Name);

            if (isExist)
                throw new BusinessValidationException([new ValidationFailure(nameof(usefulLinkDto.Name), "Link with this name already exists.")]);

            return await base.AddAsync(usefulLinkDto);
        }

        public override async Task UpdateAsync(UsefulLinkUpdateDTO usefulLinkDto)
        {
            bool isExist = await _uoW.UsefulLinkRepository.AnyAsync(u => u.Id != usefulLinkDto.Id &&
                                                                     u.Status &&
                                                                     u.Name == usefulLinkDto.Name);

            if (isExist)
                throw new BusinessValidationException([new ValidationFailure(nameof(usefulLinkDto.Name), "Link with this name already exists.")]);

            await base.UpdateAsync(usefulLinkDto);
        }
    }
}
