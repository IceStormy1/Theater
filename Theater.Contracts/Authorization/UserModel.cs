using System;

namespace Theater.Contracts.Authorization;

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