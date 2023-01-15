using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Authorization.Models;
using Theater.Abstractions.UserAccount;
using Theater.Abstractions.UserAccount.Models;
using Theater.Common;
using Theater.Entities.Authorization;

namespace Theater.Sql.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly TheaterDbContext _theaterDbContext;

        public UserAccountRepository(TheaterDbContext theaterDbContext)
        {
            _theaterDbContext = theaterDbContext;
        }

        public async Task<UserEntity> GetUserById(Guid userId)
            => await _theaterDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Id == userId);

        public async Task<UserEntity> FindUser(string userName, string password)
            => await _theaterDbContext.Users
                .AsNoTracking()
                .Include(x => x.UserRole)
                .FirstOrDefaultAsync(user => string.Equals(password, user.Password)
                                             && (string.Equals(user.UserName, userName)
                                                 || string.Equals(user.Email, userName)));

        public async Task<IReadOnlyCollection<UserEntity>> GetUsers()
            => await _theaterDbContext.Users
                .AsNoTracking()
                .OrderBy(x => x.UserName)
                .Take(300)
                .ToListAsync();

        public async Task<WriteResult<CreateUserResult>> CreateUser(UserEntity userEntity)
        {
            var user = await _theaterDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => string.Equals(userEntity.UserName, user.UserName));

            if (user != null)
                return WriteResult<CreateUserResult>.FromError(UserAccountErrors.UserAlreadyExist.Error);

            _theaterDbContext.Users.Add(userEntity);
            await _theaterDbContext.SaveChangesAsync();

            return new WriteResult<CreateUserResult>(new CreateUserResult { UserId = userEntity.Id, IsSuccess = true });
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