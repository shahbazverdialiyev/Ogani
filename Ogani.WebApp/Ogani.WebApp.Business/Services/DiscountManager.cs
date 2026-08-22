using AutoMapper;
using FluentValidation;
using Microsoft.Identity.Client;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
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
