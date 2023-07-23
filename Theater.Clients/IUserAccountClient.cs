using Refit;
using Theater.Contracts.UserAccount;

namespace Theater.Clients;

/// <summary>
/// Клиент для работы с аккаунтом пользователя
/// </summary>
public interface IUserAccountClient
{
    private const string BaseUrl = "/api/account/";

    /// <summary>
    /// Возвращает пользователя по идентификатору
    /// </summary>
    /// <returns></returns>
    /// <param name="userId">Идентификатор пользователя</param>
    [Get(BaseUrl + "user/{userId}")]
    Task<UserModel> GetUserById(Guid userId);
}