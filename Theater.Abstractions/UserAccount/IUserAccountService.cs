using System;
using System.Threading.Tasks;
using Theater.Abstractions.Authorization.Models;
using Theater.Common;
using Theater.Contracts.Authorization;
using Theater.Contracts.UserAccount;
using Theater.Entities.Authorization;

namespace Theater.Abstractions.UserAccount;

public interface IUserAccountService : ICrudService<UserParameters, UserEntity>
{
    /// <summary>
    /// Получить пользователя по уникальному идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Данные пользователя</returns>
    Task<UserModel> GetUserById(Guid userId);

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
    /// Пополнить баланс пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="replenishmentAmount">Сумма пополнения</param>
    Task<WriteResult> ReplenishBalance(Guid userId, decimal replenishmentAmount);
}