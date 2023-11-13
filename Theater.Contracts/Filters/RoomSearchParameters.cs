namespace Theater.Contracts.Filters;

public sealed class RoomSearchParameters : PagingSortParameters
{
    /// <summary>
    /// Поиск по названию чата или имени контакта (пользователя)
    /// </summary>
    public string Query { get; set; }
}