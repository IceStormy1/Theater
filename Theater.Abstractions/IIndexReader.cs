using System;
using System.Threading.Tasks;
using Theater.Abstractions.Filters;
using Theater.Common;
using Theater.Entities;

namespace Theater.Abstractions;

/// <summary>
/// Выполняет чтение данных
/// </summary>
/// <typeparam name="TIndex">Тип записей.</typeparam>
/// <typeparam name="TFilter">Тип фильтра</typeparam>
/// <typeparam name="TModel">Полная модель записи</typeparam>
public interface IIndexReader<TModel, TIndex, in TFilter>
    where TModel : class
    where TIndex : class, IEntity
    where TFilter : PagingSortSettings
{
    /// <summary>
    /// Возвращает найденные записи из хранилища.
    /// </summary>
    /// <param name="filter">Фильтр записей.</param>
    Task<PagingResult<TIndex>> QueryItems(TFilter filter);

    /// <summary>
    /// Возвращает запись по идентификатору
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<WriteResult<TModel>> GetById(Guid id);
}