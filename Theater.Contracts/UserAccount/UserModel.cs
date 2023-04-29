using System;

namespace Theater.Contracts.UserAccount;

public sealed class UserModel : UserParameters
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Дата создания пользователя
    /// </summary>
    public DateTime DateOfCreate { get; set; }
}