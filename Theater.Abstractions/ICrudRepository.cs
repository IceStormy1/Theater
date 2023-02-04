using System;
using System.Threading.Tasks;
using Theater.Entities;

namespace Theater.Abstractions
{
    public interface ICrudRepository<T> where T : IEntity
    {
        /// <summary>
        /// Получить сущность по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns></returns>
        Task<T> GetById(Guid id);

        /// <summary>
        /// Добавить сущность
        /// </summary>
        /// <param name="entity">Добавляемая сущность</param>
        /// <returns></returns>
        Task Add(T entity);

        /// <summary>
        /// Обновить сущность
        /// </summary>
        /// <param name="entity">Обновляемая сущность</param>
        /// <returns></returns>
        Task Update(T entity);

        /// <summary>
        /// Удалить сущность
        /// </summary>
        /// <param name="id">Идентификатор удаляемой сущности</param>
        /// <returns></returns>
        Task Delete(Guid id);
    }
}
