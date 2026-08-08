using AutoMapper;
using FluentValidation;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.DiscountDTO;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services
{
    public class DiscountManager : GenericManager<Discount, DiscountReadDTO, DiscountDetailReadDTO, DiscountCreateDTO, DiscountUpdateDTO>, IDiscountService
    {
        public DiscountManager(IUoW uoW, IMapper mapper, IValidator<DiscountCreateDTO> createValidator, IValidator<DiscountUpdateDTO> updateValidator) : base(uoW, mapper, createValidator, updateValidator)
        {
        }
    }
}
