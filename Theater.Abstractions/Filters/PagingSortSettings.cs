namespace Theater.Abstractions.Filters;

/// <summary>
/// Настройки пагинации.
/// </summary>
public class PagingSettings
{
    public const int DefaultLimit = 15;
    /// <summary>
    /// Максимальное количество записей, которое должен вернуть запрос.
    /// </summary>
    /// <example>15</example>
    public int Limit { get; set; } = DefaultLimit;
    /// <summary>
    /// Смещение.
    /// </summary>
    public int Offset { get; set; }
}

/// <summary>
/// Настройки пагинации с сортировкой.
/// </summary>
public class PagingSortSettings : PagingSettings
{
    /// <summary>
    /// Колонка для сортировки.
    /// </summary>
    /// <example>null</example>
    public string SortColumn { get; set; }

    /// <summary>
    /// Порядок сортировки.
    /// </summary>
    public SortOrder SortOrder { get; set; } = SortOrder.Asc;
}

public enum SortOrder
{
    Asc,
    Desc
}