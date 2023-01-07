using System;
using System.Threading.Tasks;
using Theater.Common;

namespace Theater.Abstractions.UserAccount
{
    public interface IUserAccountService
    {
        /// <summary>
        /// Пополнить баланс пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="replenishmentAmount">Сумма пополнения</param>
        Task<WriteResult> ReplenishBalance(Guid userId, decimal replenishmentAmount);
    }
}
