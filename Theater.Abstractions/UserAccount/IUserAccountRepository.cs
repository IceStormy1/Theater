using System;
using System.Threading.Tasks;

namespace Theater.Abstractions.UserAccount
{
    public interface IUserAccountRepository
    {
        /// <summary>
        /// Пополнить баланс пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="replenishmentAmount">Сумма пополнения</param>
        Task ReplenishBalance(Guid userId, decimal replenishmentAmount);
    }
}
