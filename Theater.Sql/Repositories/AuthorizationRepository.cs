using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Authorization;
using Theater.Abstractions.Authorization.Models;
using Theater.Entities.Authorization;

namespace Theater.Sql.Repositories
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly TheaterDbContext _authorizationDbContext;

        public AuthorizationRepository(TheaterDbContext authorizationDbContext)
        {
            _authorizationDbContext = authorizationDbContext;
        }

        public async Task<UserEntity> GetUserById(Guid userId)
            => await _authorizationDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Id == userId);

        public async Task<UserEntity> FindUser(string userName, string password)
            => await _authorizationDbContext.Users
                .AsNoTracking()
                .Include(x => x.UserRole)
                .FirstOrDefaultAsync(user => string.Equals(password, user.Password)
                                             && (string.Equals(user.UserName, userName)
                                                 || string.Equals(user.Email, userName)));

        public async Task<IReadOnlyCollection<UserEntity>> GetUsers()
            => await _authorizationDbContext.Users
                .AsNoTracking()
                .OrderBy(x => x.UserName)
                .Take(300)
                .ToListAsync();

        public async Task<CreateUserResult> CreateUser(UserEntity userEntity)
        {
            var user = await _authorizationDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => string.Equals(userEntity.UserName, user.UserName));

            if (user != null)
                return new CreateUserResult();

            _authorizationDbContext.Users.Add(userEntity);
            await _authorizationDbContext.SaveChangesAsync();

            return new CreateUserResult{UserId = userEntity.Id, IsSuccess = true};
        }
    }
}
