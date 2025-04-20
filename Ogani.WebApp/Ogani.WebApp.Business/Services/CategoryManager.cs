using AutoMapper;
using FluentValidation;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.CategoryDTO;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services
{
    public class CategoryManager:GenericManager<CategoryReadDTO, CategoryCreateDTO, CategoryUpdateDTO, Category>, ICategoryService
    {
        private readonly IUoW _uow;
        private readonly IMapper _mapper;
        public CategoryManager(IUoW uow, IMapper mapper, IValidator<CategoryCreateDTO> createValidator, IValidator<CategoryUpdateDTO> updateValidator)
            : base(uow, mapper, createValidator, updateValidator)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<List<CategoryReadDTO>> GetCategoriesWithProductsAsync()
        {
            List<Category> categories = await _uow.GetCategoryRepository().GetCategoriesWithProductsAsync();
            return await _mapper.Map<Task<List<CategoryReadDTO>>>(categories);
        }
    }
}
