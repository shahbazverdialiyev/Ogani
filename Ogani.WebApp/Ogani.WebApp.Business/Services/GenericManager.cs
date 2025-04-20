using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogani.WebApp.Business.Services
{
    public class GenericManager<TRead, TCreate, TUpdate, TEntity> : IService<TRead, TCreate, TUpdate>
        where TRead : class
        where TCreate : class
        where TUpdate : class
        where TEntity : BaseEntity<int>
    {
        private readonly IUoW _uoW;
        private readonly IMapper _mapper;
        private readonly IValidator<TCreate> _createValidator;
        private readonly IValidator<TUpdate> _updateValidator;

        public GenericManager(IUoW uoW, IMapper mapper, IValidator<TCreate> createValidator, IValidator<TUpdate> updateValidator) 
        { 
            _uoW = uoW;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<TRead> GetByIdAsync(int id)
        {
            TEntity entity = await _uoW.GetRepository<TEntity, int>().GetByIdAsync(id);
            return _mapper.Map<TRead>(entity);
        }

        public async Task<List<TRead>> GetAllAsync()
        {
            List<TEntity> entities = await _uoW.GetRepository<TEntity, int>().GetAllAsync();
            return _mapper.Map<List<TEntity>, List<TRead>>(entities);
        }

        public async Task AddAsync(TCreate entity)
        {
            Task<ValidationResult> validationResult = _createValidator.ValidateAsync(entity);
            if (!validationResult.Result.IsValid)
            {
                throw new ValidationException(validationResult.Result.Errors);
            }
            TEntity mappedEntity = _mapper.Map<TCreate, TEntity>(entity);
            await _uoW.GetRepository<TEntity, int>().AddAsync(mappedEntity);
            await _uoW.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            TEntity entity = await _uoW.GetRepository<TEntity, int>().GetByIdAsync(id);
            if (entity != null)
            {
                _uoW.GetRepository<TEntity, int>().Delete(entity);
                await _uoW.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(TUpdate entity)
        {
            Task<ValidationResult> validationResult = _updateValidator.ValidateAsync(entity);
            if (!validationResult.Result.IsValid)
            {
                throw new ValidationException(validationResult.Result.Errors);
            }
            TEntity mappedEntity = _mapper.Map<TUpdate, TEntity>(entity);
            _uoW.GetRepository<TEntity, int>().Update(mappedEntity);
            await _uoW.SaveChangesAsync();
        }
    }
}