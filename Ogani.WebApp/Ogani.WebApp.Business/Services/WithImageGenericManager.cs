using AutoMapper;
using FluentValidation;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.Base;
using Ogani.WebApp.Entities;

namespace Ogani.WebApp.Business.Services
{
    public class WithImageGenericManager<TEntity, TRead, TDetailRead, TCreate, TUpdate>
        : GenericManager<TEntity, TRead, TDetailRead, TCreate, TUpdate>
        where TEntity : BaseEntity<int>, IImageEntity
        where TRead : BaseDTO<int>
        where TDetailRead : BaseDTO<int>
        where TCreate : class, IWithImageDTO
        where TUpdate : BaseDTO<int>, IWithImageDTO
    {
        protected readonly IFileService _fileService;
        protected readonly string _imageFolderName;

        public WithImageGenericManager(IUoW uoW, IMapper mapper, IValidator<TCreate> createValidator, IValidator<TUpdate> updateValidator, IFileService fileService, string imageFolderName)
            : base(uoW, mapper, createValidator, updateValidator)
        {
            _fileService = fileService;
            _imageFolderName = imageFolderName;
        }

        protected override async Task PrepareEntityForCreateAsync(TEntity entity, TCreate dto)
        {
            await base.PrepareEntityForCreateAsync(entity, dto);

            if (dto.Image is not null)
                entity.ImageUrl = await _fileService.UploadAsync(dto.Image, _imageFolderName);
        }

        protected override async Task PrepareEntityForUpdateAsync(TEntity entity, TUpdate dto)
        {
            await base.PrepareEntityForUpdateAsync(entity, dto);

            if (dto is IRemovableImageDTO removableDto && removableDto.RemoveExistingImage && dto.Image is null)
            {
                if (!string.IsNullOrEmpty(entity.ImageUrl))
                {
                    await _fileService.DeleteAsync(entity.ImageUrl);
                    entity.ImageUrl = null;
                }
            }
            else if (dto.Image is not null)
            {
                if (!string.IsNullOrEmpty(entity.ImageUrl))
                {
                    await _fileService.DeleteAsync(entity.ImageUrl);
                }

                entity.ImageUrl = await _fileService.UploadAsync(dto.Image, _imageFolderName);
            }
        }

        protected override async Task PrepareEntityForDeleteAsync(TEntity entity)
        {
            await base.PrepareEntityForDeleteAsync(entity);

            if (!string.IsNullOrEmpty(entity.ImageUrl))
                await _fileService.DeleteAsync(entity.ImageUrl);
        }
    }
}