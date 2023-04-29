using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Authorization.Models;
using Theater.Abstractions.Errors;
using Theater.Abstractions.UserAccount;
using Theater.Common;
using Theater.Entities.Authorization;

namespace Theater.Sql.Repositories;

public sealed class UserAccountRepository : BaseCrudRepository<UserEntity>, IUserAccountRepository
{
    private readonly TheaterDbContext _dbContext;

    public UserAccountRepository(
        TheaterDbContext dbContext, 
        ILogger<BaseCrudRepository<UserEntity>> logger) : base(dbContext, logger)
    {
        _dbContext = dbContext;
    }

    public async Task<UserEntity> FindUser(string userName, string password)
        => await _dbContext.Users
            .AsNoTracking()
            .Include(x => x.UserRole)
            .FirstOrDefaultAsync(user => string.Equals(password, user.Password)
                                         && (string.Equals(user.UserName, userName)
                                             || string.Equals(user.Email, userName)));

    public async Task<WriteResult<CreateUserResult>> CreateUser(UserEntity userEntity)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => string.Equals(userEntity.UserName, user.UserName));

        if (user != null)
            return WriteResult<CreateUserResult>.FromError(UserAccountErrors.UserAlreadyExist.Error);

        _dbContext.Users.Add(userEntity);
        await DbContext.SaveChangesAsync();

        return new WriteResult<CreateUserResult>(new CreateUserResult { UserId = userEntity.Id, IsSuccess = true });
    }

    public async Task<WriteResult> UpdateUser(UserEntity userEntity)
    {
        _dbContext.Users.Update(userEntity);
        await DbContext.SaveChangesAsync();

        return WriteResult.Successful;
    }

    public async Task<WriteResult> ReplenishBalance(Guid userId, decimal replenishmentAmount)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user is null)
            return UserAccountErrors.NotFound;

        user.Money += replenishmentAmount;

        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        return WriteResult.Successful;
    }
}