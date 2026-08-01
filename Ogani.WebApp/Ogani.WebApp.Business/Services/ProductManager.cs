using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Ogani.WebApp.Business.Exceptions;
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
    public class ProductManager : GenericManager<Product, ProductReadDTO, ProductDetailReadDTO, ProductCreateDTO, ProductUpdateDTO>, IProductService
    {
        private readonly IFileService _fileService;
        public ProductManager(IUoW uow, IMapper mapper, IValidator<ProductCreateDTO> createValidator, IValidator<ProductUpdateDTO> updateValidator, IFileService fileService)
            : base(uow, mapper, createValidator, updateValidator)
        {
            _fileService = fileService;
        }

        public override async Task AddAsync(ProductCreateDTO dto)
        {
            ValidationResult validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            await ValidateCategoryIdAsync(dto.CategoryId);

            Product product = _mapper.Map<Product>(dto);

            if (dto.Image is not null)
                product.ImageUrl = await _fileService.UploadAsync(dto.Image, "products");

            product.Discounts = await GetDiscountsAsync(dto.DiscountIds);

            await _uoW.ProductRepository.AddAsync(product);
            await _uoW.SaveChangesAsync();
        }

        public override async Task UpdateAsync(ProductUpdateDTO dto)
        {
            ValidationResult validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            Product product = await _uoW.ProductRepository.GetByIdAsync(dto.Id, tracking: true)
                ?? throw new NotFoundException(nameof(Product), dto.Id);

            await ValidateCategoryIdAsync(dto.CategoryId);

            _mapper.Map(dto, product);

            if (dto.Image is not null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                    await _fileService.DeleteAsync(product.ImageUrl);
                product.ImageUrl = await _fileService.UploadAsync(dto.Image, "products");
            }

            product.Discounts = await GetDiscountsAsync(dto.DiscountIds);

            _uoW.ProductRepository.Update(product);
            await _uoW.SaveChangesAsync();
        }

        private async Task ValidateCategoryIdAsync(int? categoryId)
        {
            if (!categoryId.HasValue)
                return;

            bool categoryExists = await _uoW.CategoryRepository.AnyAsync(c => c.Id == categoryId.Value);

            if (!categoryExists)
                throw new NotFoundException(nameof(Category), categoryId.Value);
        }

        private async Task<List<Discount>> GetDiscountsAsync(ICollection<int> discountIds)
        {
            if (discountIds.Count == 0)
                return [];

            List<Discount> discounts = await _uoW.DiscountRepository.GetWhereAsync(d => discountIds.Contains(d.Id), tracking: true);

            if (discountIds.Count != discounts.Count)
                throw new NotFoundException("One or more selected discounts were not found.");

            return discounts;
        }

        public async Task<List<ProductReadDTO>> GetProductsByCategoryIdAsync(int categoryId)
        {
            List<Product> products = await _uoW.ProductRepository.GetProductsByCategoryIdAsync(categoryId);

            List<ProductReadDTO> productsDto = _mapper.Map<List<ProductReadDTO>>(products);
            return productsDto;
        }
    }
}
