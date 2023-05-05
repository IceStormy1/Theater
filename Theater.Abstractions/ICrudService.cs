using System;
using System.Threading.Tasks;
using Theater.Common;
using Theater.Contracts;

namespace Theater.Abstractions;

public interface ICrudService<in TModel> where TModel : class
{
    /// <summary>
    /// Добавить сущность
    /// </summary>
    /// <param name="model">Добавляемая/редактируемая сущность</param>
    /// <param name="entityId">Идентификатор сущности</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <remarks>
    /// Идентификатор<paramref name="entityId"/> заполняется при редактировании сущности
    /// </remarks>
    /// <returns></returns>
    Task<WriteResult<DocumentMeta>> CreateOrUpdate(TModel model, Guid? entityId, Guid? userId = null);

    /// <summary>
    /// Удалить сущность
    /// </summary>
    /// <param name="id">Идентификатор удаляемой сущности</param>
    /// <param name="userId">Идентификатор пользователя, который вносит изменения</param>
    /// <returns></returns>
    Task<WriteResult> Delete(Guid id, Guid? userId = null);
}