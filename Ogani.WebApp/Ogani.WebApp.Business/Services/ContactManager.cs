using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.Client.ContactDTO;
using Ogani.WebApp.DTOs.ContactDTO;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services
{
    public class ContactManager : GenericManager<Contact, ContactReadDTO, ContactDetailReadDTO, ContactCreateDTO, ContactUpdateDTO>, IContactService
    {
        public ContactManager(IUoW uoW, IMapper mapper, IValidator<ContactCreateDTO> createValiadtor, IValidator<ContactUpdateDTO> updateValidator)
            : base(uoW, mapper, createValiadtor, updateValidator) { }

        public async Task<IReadOnlyCollection<ContactDetailDTO>> GetContactsForUIAsync()
        {
            return await GetRepository.GetQuery().Where(c => c.Status).Select(c => new ContactDetailDTO()
            {
                Title = c.Title,
                Content = c.Content,
                Icon = c.Icon
            }).ToListAsync() ?? [];
        }

        public async Task<ContactDetailDTO> GetPhoneAsync()
        {
            return await GetRepository.GetQuery().Where(c => c.Status && c.Title.ToLower().Trim() == "phone")
                .Select(p => new ContactDetailDTO
                {
                    Title = p.Title,
                    Content = p.Content,
                    Icon = p.Icon
                }).FirstOrDefaultAsync()
                ?? throw new NotFoundException("Active phone was not found.");
        }

        public async Task<ContactDetailDTO> GetEmailAsync()
        {
            return await GetRepository.GetQuery().Where(c => c.Status && c.Title.ToLower().Trim() == "email")
                .Select(p => new ContactDetailDTO
                {
                    Title = p.Title,
                    Content = p.Content,
                    Icon = p.Icon
                }).FirstOrDefaultAsync()
                ?? throw new NotFoundException("Active email was not found.");
        }

        protected override async Task<List<ValidationFailure>> AddValidationFailureForCreateAsync(ContactCreateDTO contactDto)
        {
            bool isExist = await _uoW.ContactRepository.AnyAsync(c => c.Status && c.Title.Equals(contactDto.Title));

            return isExist ? [new ValidationFailure(nameof(contactDto.Title), "Contact with this title already exists.")] : [];
        }

        protected override async Task<List<ValidationFailure>> AddValidationFailureForUpdateAsync(ContactUpdateDTO contactDto)
        {
            bool isExist = await _uoW.ContactRepository.AnyAsync(c => c.Id != contactDto.Id &&
                                                                      c.Status &&
                                                                      c.Title.Equals(contactDto.Title));

            return isExist ? [new ValidationFailure(nameof(contactDto.Title), "Contact with this title already exists.")] : [];
        }

    }
}
