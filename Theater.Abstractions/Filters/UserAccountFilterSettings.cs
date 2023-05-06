namespace Theater.Abstractions.Filters;

public sealed class UserAccountFilterSettings : PagingSortSettings
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string FirstName { get; set; }
}