using System;
using System.Threading.Tasks;
using Theater.Abstractions.UserAccount;
using Theater.Common;

namespace Theater.Core.UserAccount
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAccountRepository;

        public UserAccountService(IUserAccountRepository userAccountRepository)
        {
            _userAccountRepository = userAccountRepository;
        }

        public async Task<WriteResult> ReplenishBalance(Guid userId, decimal replenishmentAmount)
        {
            return await _userAccountRepository.ReplenishBalance(userId, replenishmentAmount);
        }
    }
}
