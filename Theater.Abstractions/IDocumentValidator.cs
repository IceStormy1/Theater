using System;
using System.Threading.Tasks;
using Theater.Common;

namespace Theater.Abstractions
{
    public interface IDocumentValidator<in TModel> where TModel : class
    {
        /// <summary>
        /// Выполняет проверку можно ли создать сущность
        /// </summary>
        /// <param name="parameters">Данные сущности</param>
        Task<WriteResult> CheckIfCanCreate(TModel parameters);

        /// <summary>
        /// Выполняет проверку можно ли изменить сущность
        /// </summary>
        /// <param name="entityId">Идентификатор сущности.</param>
        /// <param name="parameters">Данные сущности</param>
        Task<WriteResult> CheckIfCanUpdate(Guid entityId, TModel parameters);

        /// <summary>
        /// Выполняет проверку можно ли удалить сущность
        /// </summary>
        /// <param name="entityId">Идентификатор сущности</param>
        Task<WriteResult> CheckIfCanDelete(Guid entityId);
    }
}