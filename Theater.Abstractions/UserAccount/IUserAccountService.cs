using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Theater.Common;
using Theater.Contracts.UserAccount;

namespace Theater.Abstractions.UserAccount;

public interface IUserAccountService : ICrudService<UserParameters>
{
    /// <summary>
    /// Обновить профиль пользователя в ЛК
    /// </summary>
    /// <param name="user">Данные пользователя</param>
    /// <param name="userId">Идентификатор пользователя</param>
    Task<Result> UpdateUserProfile(UserParameters user, Guid userId);

    /// <summary>
    /// Создать или обновить профиль пользователя исходя из токена
    /// </summary>
    /// <param name="userClaims"></param>
    Task<Result<UserModel>> CreateOrUpdateUser(ClaimsPrincipal userClaims);

    /// <summary>
    /// Пополнить баланс пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="replenishmentAmount">Сумма пополнения</param>
    Task<Result> ReplenishBalance(Guid userId, decimal replenishmentAmount);

    /// <summary>
    /// Получить идентификатор пользователя по его внешнему идентификатору
    /// </summary>
    /// <param name="externalId">Внешний идентификатор пользователя</param>
    Task<Guid?> GetUserIdByExternalId(Guid externalId);
}