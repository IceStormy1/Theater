using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Common;
using Theater.Contracts;
using Theater.Entities;

namespace Theater.Core;

public class BaseService<TModel, TEntity> : ICrudService<TModel>
    where TEntity : class, IEntity, new()
    where TModel : class
{
    protected readonly IMapper Mapper;
    protected readonly ICrudRepository<TEntity> Repository;
    protected readonly IDocumentValidator<TModel> DocumentValidator;
    protected readonly ILogger<BaseService<TModel, TEntity>> Logger;

    public BaseService(
        IMapper mapper, 
        ICrudRepository<TEntity> repository,
        IDocumentValidator<TModel> documentValidator, 
        ILogger<BaseService<TModel, TEntity>> logger)
    {
        Mapper = mapper;
        Repository = repository;
        DocumentValidator = documentValidator;
        Logger = logger;
    }

    public async Task<Result<DocumentMeta>> CreateOrUpdate(TModel model, Guid? entityId, Guid? userId = null)
    {
        var entity = entityId.HasValue 
            ? await Repository.GetByEntityId(entityId.Value) 
            : null;

        return entity is null 
            ? await CreateEntity(model) 
            : await UpdateEntity(entity, model, entityId);
    }

    public async Task<Result> Delete(Guid id, Guid? userId = null)
    {
        var isEntityExists = await Repository.IsEntityExists(id);

        if(!isEntityExists)
            return Result.FromError(ErrorModel.Default("delete-conflict", "Указанная запись не найдена"));

        var validationResult = await DocumentValidator.CheckIfCanDelete(id, userId);

        if (!validationResult.IsSuccess)
            return validationResult;

        await Repository.Delete(id);

        return Result.Successful;
    }

    private async Task<Result<DocumentMeta>> CreateEntity(TModel model)
    {
        var validationResult = await DocumentValidator.CheckIfCanCreate(model);

        if (!validationResult.IsSuccess)
            return Result<DocumentMeta>.FromError(validationResult.Error);

        var entity = Mapper.Map<TEntity>(model);

        await Repository.Add(entity);

        return Result.FromValue(new DocumentMeta(entity.Id));
    }

    private async Task<Result<DocumentMeta>> UpdateEntity(TEntity entity, TModel model, Guid? entityId)
    {
        if (!entityId.HasValue)
            return Result<DocumentMeta>.FromError(ErrorModel.Default("update-conflict", "При обновлении сущности необходимо передавать идентификатор"));

        if(entity is null)
            return Result<DocumentMeta>.FromError(ErrorModel.Default("update-conflict", "Указанная запись не найдена"));

        var validationResult = await DocumentValidator.CheckIfCanUpdate(entityId.Value, model);

        if (!validationResult.IsSuccess)
            return Result<DocumentMeta>.FromError(validationResult.Error);

        Mapper.Map(model, entity);

        await Repository.Update(entity);

        return Result.FromValue(new DocumentMeta(entity.Id));
    }
}