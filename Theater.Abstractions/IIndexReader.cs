using System.Threading.Tasks;
using Theater.Abstractions.Filter;
using Theater.Entities;

namespace Theater.Abstractions
{
    /// <summary>
    /// Выполняет чтение данных
    /// </summary>
    /// <typeparam name="TIndex">Тип записей.</typeparam>
    /// <typeparam name="TFilter">Тип фильтра</typeparam>
    public interface IIndexReader<TIndex, in TFilter>
        where TIndex : class, IEntity
        where TFilter : PagingSortSettings
    {
        /// <summary>
        /// Возвращает найденные записи из хранилища.
        /// </summary>
        /// <param name="filter">Фильтр записей.</param>
        Task<PagingResult<TIndex>> QueryItems(TFilter filter);
    }
}
