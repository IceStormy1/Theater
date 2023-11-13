namespace Theater.Common.Settings;

public sealed class ConsumerPrefetchSettings
{
    /// <summary>
    /// Название консьюмера
    /// </summary>
    public string ConsumerName { get; set; } = string.Empty;

    /// <summary>
    /// Количество одновременно обрабатываемых сообщений
    /// </summary>
    public int? PrefetchCount { get; set; }
}