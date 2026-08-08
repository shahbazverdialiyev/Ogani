using AutoMapper;
using FluentValidation;
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
    public class ContactManager : GenericManager<Contact, ContactReadDTO, ContactReadDTO, ContactCreateDTO, ContactUpdateDTO>, IContactService
    {
        public ContactManager(IUoW uoW, IMapper mapper, IValidator<ContactCreateDTO> createValiadtor, IValidator<ContactUpdateDTO> updateValidator)
            : base(uoW, mapper, createValiadtor, updateValidator) { }
    }
}
