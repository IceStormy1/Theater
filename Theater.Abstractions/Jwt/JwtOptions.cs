namespace Theater.Abstractions.Jwt;

public sealed class JwtOptions
{
    /// <summary>
    /// Кто генирирует токен 
    /// </summary>
    public string Authority { get; set; }

    /// <summary>
    /// Для кого предназначается токен
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// Секретная строка, которая будет использоваться для генерации ключа симметричного шифрования
    /// </summary>
    public string ClientSecret { get; set; }
}