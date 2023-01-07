using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Theater.Abstractions.UserAccount;
using Theater.Abstractions.UserAccount.Models;
using Theater.Common;

namespace Theater.Sql.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly TheaterDbContext _theaterDbContext;

        public UserAccountRepository(TheaterDbContext theaterDbContext)
        {
            _theaterDbContext = theaterDbContext;
        }

        public async Task<WriteResult> ReplenishBalance(Guid userId, decimal replenishmentAmount)
        {
            var user = await _theaterDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
                return UserAccountErrors.NotFound;

            user.Money += replenishmentAmount;

            _theaterDbContext.Users.Update(user);
            await _theaterDbContext.SaveChangesAsync();

            return WriteResult.Successful;
        }
    }
}