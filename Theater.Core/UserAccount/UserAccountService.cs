using AutoMapper;
using System;
using System.Threading.Tasks;
using Theater.Abstractions.UserAccount;
using Theater.Common;

namespace Theater.Core.UserAccount
{
    public class UserAccountService : ServiceBase<IUserAccountRepository>, IUserAccountService
    {
        public UserAccountService(IMapper mapper, IUserAccountRepository repository) : base(mapper, repository)
        {
        }

        public async Task<WriteResult> ReplenishBalance(Guid userId, decimal replenishmentAmount)
        {
            return await Repository.ReplenishBalance(userId, replenishmentAmount);
        }
    }
}
