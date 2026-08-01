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
    public class CategoryManager : GenericManager<Category, CategoryReadDTO, CategoryCreateDTO, CategoryUpdateDTO>, ICategoryService
    {
        public CategoryManager(IUoW uow, IMapper mapper, IValidator<CategoryCreateDTO> createValidator, IValidator<CategoryUpdateDTO> updateValidator)
            : base(uow, mapper, createValidator, updateValidator) { }
        public async Task<List<CategoryReadDTO>> GetCategoriesWithProductsAsync()
        {
            List<Category> categories = await _uoW.CategoryRepository.GetCategoriesWithProductsAsync();
            return await _mapper.Map<Task<List<CategoryReadDTO>>>(categories);
        }
    }
}
