using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Identity.Client;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.CategoryDTO;
using Ogani.WebApp.DTOs.DiscountDTO;
using Ogani.WebApp.DTOs.ProductDTO;
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
        public DiscountManager(IUoW uoW, IMapper mapper, IValidator<DiscountCreateDTO> createValidator, IValidator<DiscountUpdateDTO> updateValidator)
            : base(uoW, mapper, createValidator, updateValidator) { }

        public override async Task<int> AddAsync(DiscountCreateDTO discountDto)
        {
            ValidationResult validationResult = await _createValidator.ValidateAsync(discountDto);

            if (await _uoW.DiscountRepository.AnyAsync(d => d.Code == discountDto.Code))
                validationResult.Errors.Add(new ValidationFailure(nameof(discountDto.Code), "Discount with this code name already exists."));

            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            Discount discount = _mapper.Map<Discount>(discountDto);

            await _uoW.GetRepository<Discount, int>().AddAsync(discount);
            await _uoW.SaveChangesAsync();

            return discount.Id;
        }

        public override async Task UpdateAsync(DiscountUpdateDTO discountDto)
        {
            ValidationResult validationResult = await _updateValidator.ValidateAsync(discountDto);

            if (await _uoW.DiscountRepository.AnyAsync(c => c.Code == discountDto.Code && c.Id != discountDto.Id))
                validationResult.Errors.Add(new ValidationFailure(nameof(discountDto.Code), "Category with this name already exists."));

            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            Discount discount = await _uoW.DiscountRepository.GetByIdAsync(discountDto.Id)
                ?? throw new NotFoundException(nameof(Discount), discountDto.Id);

            _mapper.Map(discountDto, discount);

            _uoW.DiscountRepository.Update(discount);
            await _uoW.SaveChangesAsync();
        }

        public async Task<DiscountProductsDTO> GetProductsForManageAsync(int discountId)
        {
            Discount discount = await _uoW.DiscountRepository.GetByIdWithProductsAsync(discountId)
                ?? throw new NotFoundException(nameof(Discount), discountId);

            IReadOnlyCollection<Product> products = await _uoW.ProductRepository.GetAllAsync();

            return new DiscountProductsDTO
            {
                DiscountId = discount.Id,
                Products = _mapper.Map<IReadOnlyCollection<ProductReadDTO>>(products),
                SelectedProductIds = discount.Products
                    .Select(x => x.Id)
                    .ToList()
            };
        }

        public async Task UpdateProductsAsync(int discountId, ICollection<int> ProductIds)
        {
            Discount discount = await _uoW.DiscountRepository.GetByIdWithProductsAsync(discountId, tracking: true)
                ?? throw new NotFoundException(nameof(Discount), discountId);

            discount.Products.Clear();

            foreach (var product in await GetProductsAsync(ProductIds))
            {
                discount.Products.Add(product);
            }

            await _uoW.SaveChangesAsync();
        }

        private async Task<List<Product>> GetProductsAsync(ICollection<int> productIds)
        {
            if (productIds.Count == 0)
                return [];

            List<Product> products = await _uoW.ProductRepository.GetWhereAsync(p => productIds.ToHashSet().Contains(p.Id), tracking: true);

            if (productIds.Count != products.Count)
                throw new NotFoundException("One or more selected products were not found.");

            return products;
        }
    }
}
