namespace Theater.Contracts.Filters;

public sealed class UserAccountFilterParameters : PagingSortParameters
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string FirstName { get; set; }
}