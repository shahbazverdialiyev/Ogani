using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DataAccess.Interfaces;
using Ogani.WebApp.DataAccess.UnitOfWork;
using Ogani.WebApp.DTOs.Base;
using Ogani.WebApp.Entities;

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

        public virtual async Task<TDetailRead> GetByIdAsync(int id)
        {
            TEntity? entity = await GetEntityAsync(id)
                ?? throw new NotFoundException(typeof(TEntity).Name, id);

            return _mapper.Map<TDetailRead>(entity);
        }

        public virtual async Task<IReadOnlyCollection<TRead>> GetAllAsync()
        {
            IReadOnlyCollection<TEntity> entities = await GetAllEntityAsync();
            return _mapper.Map<IReadOnlyCollection<TRead>>(entities);
        }

        public virtual async Task<TUpdate> GetForUpdateAsync(int id)
        {
            TEntity entity = await GetEntityForUpdateAsync(id)
                ?? throw new NotFoundException(typeof(TEntity).Name, id);

            return _mapper.Map<TUpdate>(entity);
        }

        public virtual async Task<int> AddAsync(TCreate dto)
        {
            await ValidateForCreateAsync(dto);

            TEntity entity = _mapper.Map<TEntity>(dto);

            await PrepareEntityForCreateAsync(entity, dto);

            await GetRepository().AddAsync(entity);
            await _uoW.SaveChangesAsync();

            return entity.Id;
        }

        public virtual async Task UpdateAsync(TUpdate updatedEntity)
        {
            await ValidateForUpdateAsync(updatedEntity);

            TEntity existEntity = await GetEntityForUpdateAsync(updatedEntity.Id)
                ?? throw new NotFoundException(typeof(TEntity).Name, updatedEntity.Id);

            _mapper.Map(updatedEntity, existEntity);

            await PrepareEntityForUpdateAsync(existEntity, updatedEntity);

            GetRepository().Update(existEntity);
            await _uoW.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            TEntity entity = await GetEntityAsync(id, tracking: true)
                ?? throw new NotFoundException(typeof(TEntity).Name, id);

            await PrepareEntityForDeleteAsync(entity);

            GetRepository().Delete(entity);
            await _uoW.SaveChangesAsync();
        }

        protected virtual async Task<TEntity?> GetEntityAsync(int id, bool tracking = false)
            => await GetRepository().GetByIdAsync(id, tracking);

        protected virtual async Task<TEntity?> GetEntityForUpdateAsync(int id)
            => await GetRepository().GetForUpdateAsync(id);

        protected virtual async Task<IReadOnlyCollection<TEntity>> GetAllEntityAsync(bool tracking = false)
            => await GetRepository().GetAllAsync(tracking);

        protected virtual IRepository<TEntity, int> GetRepository() => _uoW.GetRepository<TEntity, int>();

        protected async virtual Task ValidateForCreateAsync(TCreate dto)
        {
            ValidationResult validationResult = await _createValidator.ValidateAsync(dto);

            validationResult.Errors.AddRange(await AddValidationFailureForCreateAsync(dto));

            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);
        }

        protected virtual async Task ValidateForUpdateAsync(TUpdate dto)
        {
            ValidationResult validationResult = await _updateValidator.ValidateAsync(dto);

            validationResult.Errors.AddRange(await AddValidationFailureForUpdateAsync(dto));

            if (!validationResult.IsValid)
                throw new BusinessValidationException(validationResult.Errors);
        }

        protected virtual Task<List<ValidationFailure>> AddValidationFailureForCreateAsync(TCreate dto)
            => Task.FromResult(new List<ValidationFailure>());

        protected virtual Task<List<ValidationFailure>> AddValidationFailureForUpdateAsync(TUpdate dto)
            => Task.FromResult(new List<ValidationFailure>());

        protected virtual Task PrepareEntityForCreateAsync(TEntity entity, TCreate dto) => Task.CompletedTask;
        protected virtual Task PrepareEntityForUpdateAsync(TEntity entity, TUpdate dto) => Task.CompletedTask;
        protected virtual Task PrepareEntityForDeleteAsync(TEntity entity) => Task.CompletedTask;
    }
}