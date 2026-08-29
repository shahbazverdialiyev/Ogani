using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.CategoryDTO;
using Ogani.WebApp.Entities;

namespace Ogani.WebApp.Business.Services
{
    public class CategoryManager : WithImageGenericManager<Category, CategoryReadDTO, CategoryDetailReadDTO, CategoryCreateDTO, CategoryUpdateDTO>, ICategoryService
    {
        public CategoryManager(IUoW uow, IMapper mapper, IValidator<CategoryCreateDTO> createValidator, IValidator<CategoryUpdateDTO> updateValidator, IFileService fileService)
            : base(uow, mapper, createValidator, updateValidator, fileService, imageFolderName: "categories")
        {
        }

        public async Task<List<CategoryReadDTO>> GetCategoriesWithProductsAsync()
        {
            List<Category> categories = await _uoW.CategoryRepository.GetCategoriesWithProductsAsync();
            return _mapper.Map<List<CategoryReadDTO>>(categories);
        }

        protected override async Task<List<ValidationFailure>> AddValidationFailureForCreateAsync(CategoryCreateDTO dto)
        {
            if (await _uoW.CategoryRepository.AnyAsync(c => c.Name == dto.Name))
                return [new ValidationFailure(nameof(dto.Name), "Category with this name already exists.")];

            return [];
        }

        protected override async Task<List<ValidationFailure>> AddValidationFailureForUpdateAsync(CategoryUpdateDTO dto)
        {
            if (await _uoW.CategoryRepository.AnyAsync(c => c.Name == dto.Name && c.Id != dto.Id))
                return [new ValidationFailure(nameof(dto.Name), "Category with this name already exists.")];

            return [];
        }
    }
}
