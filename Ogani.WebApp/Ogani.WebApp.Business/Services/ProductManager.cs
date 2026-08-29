using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.Business.Validators.ProductValidators;
using Ogani.WebApp.DataAccess.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.ProductDTO;
using Ogani.WebApp.Entities;

namespace Ogani.WebApp.Business.Services
{
    public class ProductManager : WithImageGenericManager<Product, ProductReadDTO, ProductDetailReadDTO, ProductCreateDTO, ProductUpdateDTO>, IProductService
    {

        public ProductManager(IUoW uow, IMapper mapper, IValidator<ProductCreateDTO> createValidator, IValidator<ProductUpdateDTO> updateValidator, IFileService fileService)
            : base(uow, mapper, createValidator, updateValidator, fileService,imageFolderName: "products")
        {
        }

        public async Task<IReadOnlyCollection<ProductReadDTO>> GetProductsByCategoryIdAsync(int categoryId)
        {
            IReadOnlyCollection<Product> products = await _uoW.ProductRepository.GetProductsByCategoryIdAsync(categoryId);

            return _mapper.Map<IReadOnlyCollection<ProductReadDTO>>(products);
        }

        public async Task<IReadOnlyCollection<ProductReadDTO>> GetProductsByDiscountIdAsync(int discountId)
        {
            IReadOnlyCollection<Product> products = await _uoW.ProductRepository.GetProductsByDiscountIdAsync(discountId);

            return _mapper.Map<IReadOnlyCollection<ProductReadDTO>>(products);
        }

        protected override IRepository<Product, int> GetRepository() => _uoW.ProductRepository;

        protected async override Task<List<ValidationFailure>> AddValidationFailureForCreateAsync(ProductCreateDTO dto)
        {
            var failures = new List<ValidationFailure>();

            if (await _uoW.ProductRepository.AnyAsync(x => x.Name == dto.Name))
                failures.Add(new ValidationFailure(nameof(dto.Name), "Product with this name already exists."));

            if (dto.CategoryId.HasValue && !await _uoW.CategoryRepository.AnyAsync(c => c.Id == dto.CategoryId.Value))
                failures.Add(new ValidationFailure(nameof(dto.CategoryId), $"Category with id {dto.CategoryId.Value} was not found."));

            return failures;
        }

        protected async override Task<List<ValidationFailure>> AddValidationFailureForUpdateAsync(ProductUpdateDTO dto)
        {
            var failures = new List<ValidationFailure>();

            if (await _uoW.ProductRepository.AnyAsync(x => x.Name == dto.Name && x.Id != dto.Id))
                failures.Add(new ValidationFailure(nameof(dto.Name), "Product with this name already exists."));

            if (dto.CategoryId.HasValue && !await _uoW.CategoryRepository.AnyAsync(c => c.Id == dto.CategoryId.Value))
                failures.Add(new ValidationFailure(nameof(dto.CategoryId), $"Category with id {dto.CategoryId.Value} was not found."));

            return failures;
        }

        protected async override Task PrepareEntityForCreateAsync(Product product, ProductCreateDTO dto)
        {
            await base.PrepareEntityForCreateAsync(product, dto);
            product.Discounts = await GetDiscountsAsync(dto.DiscountIds);
        }

        protected async override Task PrepareEntityForUpdateAsync(Product product, ProductUpdateDTO dto)
        {
            await base.PrepareEntityForUpdateAsync(product, dto);
            product.Discounts = await GetDiscountsAsync(dto.DiscountIds);
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
