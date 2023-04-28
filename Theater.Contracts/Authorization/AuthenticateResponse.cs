namespace Theater.Contracts.Authorization;

public sealed class AuthenticateResponse
{
    /// <summary>
    /// Токен
    /// </summary>
    public string AccessToken { get; set; }
}