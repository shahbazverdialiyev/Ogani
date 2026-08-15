using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Ogani.WebApp.Business.Exceptions;
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
    public class CategoryManager : GenericManager<Category, CategoryReadDTO, CategoryDetailReadDTO, CategoryCreateDTO, CategoryUpdateDTO>, ICategoryService
    {
        private readonly IFileService _fileService;

        public CategoryManager(IUoW uow, IMapper mapper, IValidator<CategoryCreateDTO> createValidator, IValidator<CategoryUpdateDTO> updateValidator, IFileService fileService)
            : base(uow, mapper, createValidator, updateValidator)
        {
            _fileService = fileService;
        }

        public override async Task<int> AddAsync(CategoryCreateDTO dto)
        {
            ValidationResult validationResult = await _createValidator.ValidateAsync(dto);

            if (await _uoW.CategoryRepository.AnyAsync(c => c.Name == dto.Name))
                validationResult.Errors.Add(new ValidationFailure(nameof(dto.Name), "Category with this name already exists."));

            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            Category category = _mapper.Map<Category>(dto);

            if (dto.Image is not null)
                category.ImageUrl = await _fileService.UploadAsync(dto.Image, "categories");

            await _uoW.CategoryRepository.AddAsync(category);
            await _uoW.SaveChangesAsync();

            return category.Id;
        }

        public override async Task UpdateAsync(CategoryUpdateDTO dto)
        {
            ValidationResult validationResult = await _updateValidator.ValidateAsync(dto);

            if (await _uoW.CategoryRepository.AnyAsync(c => c.Name == dto.Name && c.Id != dto.Id))
                validationResult.Errors.Add(new ValidationFailure(nameof(dto.Name), "Category with this name already exists."));

            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            Category category = await _uoW.CategoryRepository.GetForUpdateAsync(dto.Id)
                ?? throw new NotFoundException(nameof(Category), dto.Id);

            _mapper.Map(dto, category);

            if (dto.RemoveExistingImage && category.ImageUrl != null)
            {
                await _fileService.DeleteAsync(category.ImageUrl);
                category.ImageUrl = null;
            }

            if (dto.Image is not null)
            {
                category.ImageUrl = await _fileService.UploadAsync(dto.Image, "categories");
            }

            _uoW.CategoryRepository.Update(category);
            await _uoW.SaveChangesAsync();
        }

        public override async Task DeleteAsync(int categoryId)
        {
            Category? category = await _uoW.CategoryRepository.GetByIdAsync(categoryId, tracking: true);

            if (category == null)
                throw new NotFoundException(nameof(Category), categoryId);

            if (!string.IsNullOrEmpty(category.ImageUrl))
                await _fileService.DeleteAsync(category.ImageUrl);

            _uoW.CategoryRepository.Delete(category);
            await _uoW.SaveChangesAsync();
        }

        public async Task<List<CategoryReadDTO>> GetCategoriesWithProductsAsync()
        {
            List<Category> categories = await _uoW.CategoryRepository.GetCategoriesWithProductsAsync();
            return await _mapper.Map<Task<List<CategoryReadDTO>>>(categories);
        }
    }
}
