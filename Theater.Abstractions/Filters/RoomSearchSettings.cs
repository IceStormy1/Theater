namespace Theater.Abstractions.Filters;

public sealed class RoomSearchSettings : PagingSettings
{
    /// <summary>
    /// Поиск по названию чата или имени контакта (пользователя)
    /// </summary>
    public string Query { get; set; }
}