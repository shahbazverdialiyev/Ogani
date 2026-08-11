using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.Base;
using Ogani.WebApp.DTOs.ProductDTO;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services
{
    public class GenericManager<TEntity, TRead, TDetailRead, TCreate, TUpdate> : IService<TRead, TDetailRead, TCreate, TUpdate>
        where TEntity : BaseEntity<int>
        where TRead : BaseDTO<int>
        where TDetailRead : BaseDTO<int>
        where TCreate : class
        where TUpdate : BaseDTO<int>
    {
        protected readonly IUoW _uoW;
        protected readonly IMapper _mapper;
        protected readonly IValidator<TCreate> _createValidator;
        protected readonly IValidator<TUpdate> _updateValidator;

        public GenericManager(IUoW uoW, IMapper mapper, IValidator<TCreate> createValidator, IValidator<TUpdate> updateValidator)
        {
            _uoW = uoW;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<TDetailRead> GetByIdAsync(int id)
        {
            TEntity? entity = await _uoW.GetRepository<TEntity, int>().GetByIdAsync(id)
                ?? throw new NotFoundException(typeof(TEntity).Name, id);

            return _mapper.Map<TDetailRead>(entity);
        }

        public virtual async Task<IReadOnlyCollection<TRead>> GetAllAsync()
        {
            List<TEntity> entities = await _uoW.GetRepository<TEntity, int>().GetAllAsync();
            return _mapper.Map<List<TRead>>(entities);
        }

        public virtual async Task<TUpdate> GetForUpdateAsync(int id)
        {
            var repository = _uoW.GetRepository<TEntity, int>();

            TEntity entity = await repository.GetForUpdateAsync(id)
                ?? throw new NotFoundException(typeof(TEntity).Name, id);

            return _mapper.Map<TUpdate>(entity);
        }

        public virtual async Task AddAsync(TCreate dto)
        {
            ValidationResult validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            TEntity entity = _mapper.Map<TEntity>(dto);

            await _uoW.GetRepository<TEntity, int>().AddAsync(entity);
            await _uoW.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TUpdate updatedEntity)
        {
            ValidationResult validationResult = await _updateValidator.ValidateAsync(updatedEntity);

            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);

            var repository = _uoW.GetRepository<TEntity, int>();

            TEntity existEntity = await repository.GetByIdAsync(updatedEntity.Id, tracking: true)
                ?? throw new NotFoundException(typeof(TEntity).Name, updatedEntity.Id);

            _mapper.Map(updatedEntity, existEntity);

            repository.Update(existEntity);
            await _uoW.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var repository = _uoW.GetRepository<TEntity, int>();

            TEntity entity = await repository.GetByIdAsync(id, tracking: true)
                ?? throw new NotFoundException(typeof(TEntity).Name, id);

            repository.Delete(entity);
            await _uoW.SaveChangesAsync();
        }
    }
}