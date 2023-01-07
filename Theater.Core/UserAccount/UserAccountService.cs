using System;
using System.Threading.Tasks;
using Theater.Abstractions.UserAccount;

namespace Theater.Core.UserAccount
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAccountRepository;

        public UserAccountService(IUserAccountRepository userAccountRepository)
        {
            _userAccountRepository = userAccountRepository;
        }

        public async Task ReplenishBalance(Guid userId, decimal replenishmentAmount)
        {
            await _userAccountRepository.ReplenishBalance(userId, replenishmentAmount);
        }
    }
}
