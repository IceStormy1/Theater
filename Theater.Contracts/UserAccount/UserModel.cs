using System;
using Theater.Common.Enums;

namespace Theater.Contracts.UserAccount;

public sealed class UserModel : UserParameters
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Внешний идентификатор пользователя из сервиса авторизации
    /// </summary>
    /// <remarks>
    /// <see href="https://github.com/IceStormy1/Authorization-API"></see>
    /// </remarks>
    public Guid ExternalUserId { get; set; }

    /// <summary>
    /// Дата создания пользователя
    /// </summary>
    public DateTime DateOfCreate { get; set; }

    /// <inheritdoc cref="UserRole"/>
    public UserRole Role { get; set; }
}