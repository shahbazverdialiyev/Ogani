using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.Business.Validators.ProductValidators;
using Ogani.WebApp.DataAccess.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.Client;
using Ogani.WebApp.DTOs.Client.ProductDTO;
using Ogani.WebApp.DTOs.ProductDTO;
using Ogani.WebApp.Entities;

namespace Ogani.WebApp.Business.Services
{
    public class ProductManager : WithImageGenericManager<Product, ProductReadDTO, ProductDetailReadDTO, ProductCreateDTO, ProductUpdateDTO>, IProductService
    {

        public ProductManager(IUoW uow, IMapper mapper, IValidator<ProductCreateDTO> createValidator, IValidator<ProductUpdateDTO> updateValidator, IFileService fileService)
            : base(uow, mapper, createValidator, updateValidator, fileService, imageFolderName: "products")
        {
        }

        protected override IRepository<Product, int> GetRepository => _uoW.ProductRepository;

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

        public async Task<PagedResultDTO<ProductCardDTO>> GetProductsForShopAsync(ProductFilterDTO filter)
        {
            IQueryable<Product> query = GetRepository.GetQuery().Where(p => p.Status);

            if (!string.IsNullOrWhiteSpace(filter.Search))
                query = query.Where(p => p.Name.Contains(filter.Search));

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId);

            int minPrice = (int)await query.MinAsync(p => (double)p.Price);
            int maxPrice = (int)await query.MaxAsync(p => (double)p.Price);

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);

            query = filter.SortBy?.ToLower() switch
            {
                "price-asc" => query.OrderBy(x => x.Price),
                "price-desc" => query.OrderByDescending(x => x.Price),
                "name-asc" => query.OrderBy(x => x.Name),
                _ => query.OrderByDescending(x => x.Id)
            };

            int totalItems = await query.CountAsync();

            IReadOnlyCollection<ProductCardDTO> products =
                await query
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(p => new ProductCardDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        CategoryName = p.Category!.Name,

                        DiscountPercentage = p.Discounts
                            .Where(d => d.Status &&
                                        d.StartDate <= DateTime.UtcNow &&
                                        d.EndDate >= DateTime.UtcNow)
                            .Select(d => (decimal?)d.DiscountPercentage)
                            .Max() ?? 0
                    })
                    .ToListAsync();

            return new PagedResultDTO<ProductCardDTO>
            {
                Items = products,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                PageSize = filter.PageSize,
                TotalItems = totalItems,
                PageIndex = filter.Page,
            };
        }

        public async Task<IReadOnlyCollection<ProductCardDTO>> GetFeaturedProductsAsync()
        {
            return await GetRepository.GetQuery().Where(p => p.Status && p.IsFeatured && p.Category != null)
                .Select(p => new ProductCardDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryName = p.Category!.Name,

                    DiscountPercentage = p.Discounts.Where(d => d.Status &&
                                                           d.StartDate <= DateTime.UtcNow &&
                                                           d.EndDate >= DateTime.UtcNow)
                                                    .Max(d => (decimal?)d.DiscountPercentage) ?? 0
                }).ToListAsync();
        }

        public async Task<IReadOnlyCollection<ProductCardDTO>> GetLatestProductsAsync()
        {
            return await GetRepository.GetQuery().Where(p => p.Status).OrderBy(p => p.CreatedDate).Select(p => new ProductCardDTO()
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl

                //Discount = p.GetAvtiveDiscount()
            }).ToListAsync();
        }

        public async Task<ProductDetailDTO> GetProductDetailAsync(int id)
        {
            return await GetRepository.GetQuery().Where(p => p.Status && p.Id == id).Select(p => new ProductDetailDTO()
            {
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category!.Name,
                Description = p.Description,
                Weight = p.Weight,
                Info = p.Info,
                IsAvailable = p.IsAvailable,

                DiscountPercentage = p.Discounts.Where(d => d.Status &&
                                                           d.StartDate <= DateTime.UtcNow &&
                                                           d.EndDate >= DateTime.UtcNow)
                                                    .Max(d => (decimal?)d.DiscountPercentage) ?? 0
            }).FirstOrDefaultAsync()
            ?? throw new NotFoundException(nameof(ProductDetailDTO), id);
        }

        public async Task<IReadOnlyCollection<ProductCardDTO>> GetProductsByCategoryForUIAsync(int id)
        {
            return await GetRepository.GetQuery().Where(p => p.Status && p.CategoryId == id).Select(p => new ProductCardDTO()
            {
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl

                //Discount = p.GetAvtiveDiscount()
            }).ToListAsync();
        }

        public async Task<IReadOnlyCollection<ProductCardDTO>> GetDiscountedProductsAsync()
        {
            return await GetRepository.GetQuery()
                .Where(p => p.Status && p.Discounts.Any(d => d.Status &&
                                                             d.StartDate <= DateTime.UtcNow &&
                                                             d.EndDate >= DateTime.UtcNow))
                .Select(p => new ProductCardDTO()
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryName = p.Category != null ? p.Category.Name : "",
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    DiscountPercentage = p.Discounts.Where(d => d.Status &&
                                                           d.StartDate <= DateTime.UtcNow &&
                                                           d.EndDate >= DateTime.UtcNow)
                                                    .Max(d => (decimal?)d.DiscountPercentage) ?? 0
                }).ToListAsync();
        }

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
