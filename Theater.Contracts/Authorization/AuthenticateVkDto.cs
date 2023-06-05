namespace Theater.Contracts.Authorization;

public sealed class AuthenticateVkDto
{
    /// <summary>
    /// Токен ВК
    /// </summary>
    public string AccessToken { get; set; }
}