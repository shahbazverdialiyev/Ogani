using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
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

        public override async Task<int> AddAsync(ContactCreateDTO contactDto)
        {
            bool isExist = await _uoW.ContactRepository.AnyAsync(c => c.Status &&
                                                                      c.Title.Equals(contactDto.Title));

            if (isExist)
                throw new BusinessValidationException([new ValidationFailure(nameof(contactDto.Title), "Contact with this title already exists.")]);

            return await base.AddAsync(contactDto);
        }

        public override async Task UpdateAsync(ContactUpdateDTO contactDto)
        {
            bool isExist = await _uoW.ContactRepository.AnyAsync(c =>c.Id != contactDto.Id &&
                                                                     c.Status &&
                                                                     c.Title.Equals(contactDto.Title));

            if (isExist)
                throw new BusinessValidationException([new ValidationFailure(nameof(contactDto.Title), "Contact with this title already exists.")]);

            await base.UpdateAsync(contactDto);
        }
    }
}
