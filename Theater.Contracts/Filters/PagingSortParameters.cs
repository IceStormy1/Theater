namespace Theater.Contracts.Filters
{
    /// <summary>
    /// Настройки пагинации.
    /// </summary>
    public class PagingParameters
    {
        public const int DefaultLimit = 15;

        /// <summary>
        /// Максимальное количество записей, которое должен вернуть запрос.
        /// </summary>
        /// <example>15</example>
        public virtual int Limit { get; set; } = DefaultLimit;

        /// <summary>
        /// Смещение.
        /// </summary>
        public int Offset { get; set; }
    }

    /// <summary>
    /// Настройки пагинации с сортировкой.
    /// </summary>
    public class PagingSortParameters : PagingParameters
    {
        /// <summary>
        /// Колонка для сортировки.
        /// </summary>
        /// <example>null</example>
        public string SortColumn { get; set; }

        /// <summary>
        /// Порядок сортировки. 0 - по возрастанию (ASC); 1 - по убыванию (DESC)
        /// </summary>
        public PagingSortOrder SortOrder { get; set; } = PagingSortOrder.Asc;
    }

    /// <summary>
    /// Порядок сортировки. 0 - по возрастанию (ASC); 1 - по убыванию (DESC)
    /// </summary>
    public enum PagingSortOrder
    {
        /// <summary>
        /// По возрастанию (ASC). 
        /// </summary>
        Asc,

        /// <summary>
        /// По убыванию (DESC).
        /// </summary>
        Desc
    }
}
