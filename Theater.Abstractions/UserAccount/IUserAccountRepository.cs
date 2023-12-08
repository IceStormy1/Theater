using System;
using System.Threading.Tasks;
using Theater.Abstractions.Authorization.Models;
using Theater.Common;
using Theater.Entities.Users;

namespace Theater.Abstractions.UserAccount;

public interface IUserAccountRepository : ICrudRepository<UserEntity>
{
    /// <summary>
    /// Получить пользователя по его никнейму
    /// </summary>
    /// <param name="userName">Никнейм пользователя</param>
    Task<UserEntity> FindUser(string userName, Guid externalUserId);

    /// <summary>
    /// Создать пользователя
    /// </summary>
    /// <param name="userEntity">Данные пользователя</param>
    Task<Result<CreateUserResult>> CreateUser(UserEntity userEntity);

    /// <summary>
    /// Создать пользователя
    /// </summary>
    /// <param name="userEntity">Данные пользователя</param>
    Task<Result> UpdateUser(UserEntity userEntity);

    /// <summary>
    /// Пополнить баланс пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="replenishmentAmount">Сумма пополнения</param>
    Task<Result> ReplenishBalance(Guid userId, decimal replenishmentAmount);

    /// <summary>
    /// Возвращает системного пользователя
    /// </summary>
    Task<Result<UserEntity>> GetSystemUser();

    /// <summary>
    /// Получить идентификатор пользователя по его внешнему идентификатору
    /// </summary>
    /// <param name="externalId">Внешний идентификатор пользователя</param>
    /// <returns></returns>
    Task<Guid?> GetUserIdByExternalId(Guid externalId);

    /// <summary>
    /// Получить сущность пользователя по его внешнему идентификатору
    /// </summary>
    /// <param name="externalId">Внешний идентификатор пользователя</param>
    Task<UserEntity> GetUserByExternalId(Guid externalId);
}