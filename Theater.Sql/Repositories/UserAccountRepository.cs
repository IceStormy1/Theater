using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Theater.Abstractions.Authorization.Models;
using Theater.Abstractions.UserAccount;
using Theater.Abstractions.UserAccount.Models;
using Theater.Common;
using Theater.Entities.Authorization;

namespace Theater.Sql.Repositories
{
    public sealed class UserAccountRepository : BaseCrudRepository<UserEntity, TheaterDbContext>, IUserAccountRepository
    {
        public UserAccountRepository(
            TheaterDbContext dbContext, 
            ILogger<BaseCrudRepository<UserEntity, TheaterDbContext>> logger) : base(dbContext, logger)
        {
        }

        public async Task<UserEntity> FindUser(string userName, string password)
            => await DbContext.Users
                .AsNoTracking()
                .Include(x => x.UserRole)
                .FirstOrDefaultAsync(user => string.Equals(password, user.Password)
                                             && (string.Equals(user.UserName, userName)
                                                 || string.Equals(user.Email, userName)));

        public async Task<IReadOnlyCollection<UserEntity>> GetUsers()
            => await DbContext.Users
                .AsNoTracking()
                .OrderBy(x => x.UserName)
                .Take(300)
                .ToListAsync();

        public async Task<WriteResult<CreateUserResult>> CreateUser(UserEntity userEntity)
        {
            var user = await DbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => string.Equals(userEntity.UserName, user.UserName));

            if (user != null)
                return WriteResult<CreateUserResult>.FromError(UserAccountErrors.UserAlreadyExist.Error);

            DbContext.Users.Add(userEntity);
            await DbContext.SaveChangesAsync();

            return new WriteResult<CreateUserResult>(new CreateUserResult { UserId = userEntity.Id, IsSuccess = true });
        }

        public async Task<WriteResult> UpdateUser(UserEntity userEntity)
        {
            DbContext.Users.Update(userEntity);
            await DbContext.SaveChangesAsync();

            return WriteResult.Successful;
        }

        public async Task<WriteResult> ReplenishBalance(Guid userId, decimal replenishmentAmount)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
                return UserAccountErrors.NotFound;

            user.Money += replenishmentAmount;

            DbContext.Users.Update(user);
            await DbContext.SaveChangesAsync();

            return WriteResult.Successful;
        }
    }
}