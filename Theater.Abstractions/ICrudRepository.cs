using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Entities;

namespace Theater.Abstractions;

public interface ICrudRepository<TEntity> where TEntity : IEntity
{
    /// <summary>
    /// Получить сущность по идентификатору
    /// </summary>
    /// <param name="entityId">Идентификатор сущности</param>
    /// <param name="useAsNoTracking">Добавлять ли AsNoTracking</param>
    /// <returns></returns>
    Task<TEntity> GetByEntityId(Guid entityId, bool useAsNoTracking = false);

    /// <summary>
    /// Получить сущности по идентификаторам
    /// </summary>
    /// <param name="entityIds">Идентификатор сущности</param>
    Task<IReadOnlyCollection<TEntity>> GetByEntityIds(IReadOnlyCollection<Guid> entityIds);

    /// <summary>
    /// Добавить сущность
    /// </summary>
    /// <param name="entity">Добавляемая сущность</param>
    /// <returns></returns>
    Task Add(TEntity entity);

    /// <summary>
    /// Добавляет сущности
    /// </summary>
    Task AddRange(IReadOnlyCollection<TEntity> entities);

    /// <summary>
    /// Обновить сущность
    /// </summary>
    /// <param name="entity">Обновляемая сущность</param>
    /// <returns></returns>
    Task Update(TEntity entity);

    /// <summary>
    /// Обновляет сущности
    /// </summary>
    Task UpdateRange(IReadOnlyCollection<TEntity> entities);

    /// <summary>
    /// Удалить сущность
    /// </summary>
    /// <param name="id">Идентификатор удаляемой сущности</param>
    /// <returns></returns>
    Task Delete(Guid id);

    /// <summary>
    /// Проверяет, существует ли сущность с таким идентификатором 
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    Task<bool> IsEntityExists(Guid id);

    IQueryable<TEntity> AddIncludes(IQueryable<TEntity> query);
}