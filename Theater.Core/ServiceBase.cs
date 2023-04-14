using AutoMapper;
using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Errors;
using Theater.Common;
using Theater.Contracts;
using Theater.Entities;

namespace Theater.Core
{
    public abstract class ServiceBase<TModel, TEntity> : ICrudService<TModel, TEntity>
        where TEntity : class, IEntity, new()
        where TModel : class
    {
        protected readonly IMapper Mapper;
        protected readonly ICrudRepository<TEntity> Repository;
        protected readonly IDocumentValidator<TModel> DocumentValidator;

        protected ServiceBase(
            IMapper mapper, 
            ICrudRepository<TEntity> repository,
            IDocumentValidator<TModel> documentValidator)
        {
            Mapper = mapper;
            Repository = repository;
            DocumentValidator = documentValidator;
        }

        public async Task<WriteResult<TModel>> GetById(Guid id)
        {
            var entity = await Repository.GetByEntityId(id);

            return entity is null
                ? WriteResult<TModel>.FromError(PieceErrors.NotFound.Error)
                : WriteResult.FromValue(Mapper.Map<TModel>(entity));
        }

        public async Task<WriteResult<DocumentMeta>> CreateOrUpdate(TModel model, Guid? entityId, Guid? userId = null)
        {
            var entity = entityId.HasValue 
                ? await Repository.GetByEntityId(entityId.Value) 
                : null;

            return entity is null 
                ? await CreateEntity(model) 
                : await UpdateEntity(entity, model, entityId);
        }

        public async Task<WriteResult> Delete(Guid id, Guid? userId = null)
        {
            var validationResult = await DocumentValidator.CheckIfCanDelete(id, userId);

            if (!validationResult.IsSuccess)
                return validationResult;

            await Repository.Delete(id);

            return WriteResult.Successful;
        }

        private async Task<WriteResult<DocumentMeta>> CreateEntity(TModel model)
        {
            var validationResult = await DocumentValidator.CheckIfCanCreate(model);

            if (!validationResult.IsSuccess)
                return WriteResult<DocumentMeta>.FromError(validationResult.Error);

            var entity = Mapper.Map<TEntity>(model);

            await Repository.Add(entity);

            return WriteResult.FromValue(new DocumentMeta(entity.Id));
        }

        private async Task<WriteResult<DocumentMeta>> UpdateEntity(TEntity entity, TModel model, Guid? entityId)
        {
            if (!entityId.HasValue)
                return WriteResult<DocumentMeta>.FromError(ErrorModel.Default("update-conflict", "При обновлении сущности необходимо передавать идентификатор"));

            if(entity is null)
                return WriteResult<DocumentMeta>.FromError(ErrorModel.Default("update-conflict", "Указанная запись не найдена"));

            var validationResult = await DocumentValidator.CheckIfCanUpdate(entityId.Value, model);

            if (!validationResult.IsSuccess)
                return WriteResult<DocumentMeta>.FromError(validationResult.Error);

            Mapper.Map(model, entity);

            await Repository.Update(entity);

            return WriteResult.FromValue(new DocumentMeta(entity.Id));
        }
    }
}
