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

        public override async Task<List<ProductReadDTO>> GetAllAsync()
        {
            List<Product> entities = await _uoW.ProductRepository.GetAllAsync();
            return _mapper.Map<List<ProductReadDTO>>(entities);
        }

        public override async Task<ProductUpdateDTO> GetForUpdateAsync(int id)
        {
            Product product = await _uoW.ProductRepository.GetForUpdateAsync(id)
                ?? throw new NotFoundException(nameof(Product), id);

            ProductUpdateDTO dto = _mapper.Map<ProductUpdateDTO>(product);

            dto.DiscountIds = product.Discounts.Select(x => x.Id).ToList();

            return dto;
        }

        public override async Task AddAsync(ProductCreateDTO dto)
        {
            ValidationResult validationResult = await _createValidator.ValidateAsync(dto);

            if (await _uoW.ProductRepository.AnyAsync(x => x.Name == dto.Name))
                validationResult.Errors.Add(new ValidationFailure("Name", "Product with this name already exists."));

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

            if (await _uoW.ProductRepository.AnyAsync(x => x.Name == dto.Name && x.Id != dto.Id))
                validationResult.Errors.Add(new ValidationFailure("Name", "Another product with this name already exists."));

            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            Product product = await _uoW.ProductRepository.GetForUpdateAsync(dto.Id)
                ?? throw new NotFoundException(nameof(Product), dto.Id);

            await ValidateCategoryIdAsync(dto.CategoryId);

            if (dto.RemoveExistingImage && dto.Image == null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    await _fileService.DeleteAsync(product.ImageUrl);
                    product.ImageUrl = null;
                }
            }

            else if (dto.Image is not null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                    await _fileService.DeleteAsync(product.ImageUrl);

                product.ImageUrl = await _fileService.UploadAsync(dto.Image, "products");
            }

            _mapper.Map(dto, product);

            product.Discounts = await GetDiscountsAsync(dto.DiscountIds);

            _uoW.ProductRepository.Update(product);
            await _uoW.SaveChangesAsync();
        }

        public override async Task DeleteAsync(int productId)
        {
            Product product = await _uoW.ProductRepository.GetByIdAsync(productId, tracking: true)
                ?? throw new NotFoundException(nameof(Product), productId);

            if (!string.IsNullOrEmpty(product.ImageUrl))
                await _fileService.DeleteAsync(product.ImageUrl);

            _uoW.ProductRepository.Delete(product);
            await _uoW.SaveChangesAsync();
        }

        public async Task<List<ProductReadDTO>> GetProductsByCategoryIdAsync(int categoryId)
        {
            List<Product> products = await _uoW.ProductRepository.GetProductsByCategoryIdAsync(categoryId);

            List<ProductReadDTO> productsDto = _mapper.Map<List<ProductReadDTO>>(products);
            return productsDto;
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
    }
}
