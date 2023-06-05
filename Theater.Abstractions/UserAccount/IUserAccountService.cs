using System;
using System.Threading.Tasks;
using Theater.Abstractions.Authorization.Models;
using Theater.Common;
using Theater.Contracts.Authorization;
using Theater.Contracts.UserAccount;

namespace Theater.Abstractions.UserAccount;

public interface IUserAccountService : ICrudService<UserParameters>
{
    /// <summary>
    /// Создать пользователя
    /// </summary>
    /// <param name="user">Данные пользователя</param>
    Task<WriteResult<CreateUserResult>> CreateUser(UserParameters user);

    /// <summary>
    /// Обновить профиль пользователя
    /// </summary>
    /// <param name="user">Данные пользователя</param>
    /// <param name="userId">Идентификатор пользователя</param>
    Task<WriteResult> UpdateUser(UserParameters user, Guid userId);

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="authenticateParameters"></param>
    Task<AuthenticateResponse> Authorize(AuthenticateParameters authenticateParameters);

    /// <summary>
    /// Авторизоваться при помощи ВКонтакте
    /// </summary>
    Task<WriteResult<AuthenticateResponse>> AuthorizeWithVk(AuthenticateVkDto  authenticateVkDto);

    /// <summary>
    /// Пополнить баланс пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="replenishmentAmount">Сумма пополнения</param>
    Task<WriteResult> ReplenishBalance(Guid userId, decimal replenishmentAmount);
}