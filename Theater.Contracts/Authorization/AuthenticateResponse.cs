using System;

namespace Theater.Contracts.Authorization;

public sealed class AuthenticateResponse
{
    /// <summary>
    /// Токен
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }
}