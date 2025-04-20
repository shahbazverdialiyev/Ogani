using AutoMapper;
using FluentValidation;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.Business.Validators.ProductValidators;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.ProductDTO;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services
{
    public class ProductManager:GenericManager<ProductReadDTO, ProductCreateDTO, ProductUpdateDTO, Product>, IProductService
    {
        private readonly IUoW _uow;
        private readonly IMapper _mapper;


        public ProductManager(IUoW uow, IMapper mapper, IValidator<ProductCreateDTO> createValidator, IValidator<ProductUpdateDTO> updateValidator)
            : base(uow, mapper, createValidator, updateValidator)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<ProductReadDTO>> GetProductsByCategoryIdAsync(int categoryId)
        {
            List<Product> products = await _uow.GetProductRepository().GetProductsByCategoryIdAsync(categoryId);
            return await _mapper.Map<Task<List<ProductReadDTO>>>(products);
        }
    }
}
