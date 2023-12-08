using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Authorization.Models;
using Theater.Abstractions.Errors;
using Theater.Abstractions.UserAccount;
using Theater.Common;
using Theater.Common.Enums;
using Theater.Entities.Users;

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

    public Task<UserEntity> FindUser(string userName, Guid externalUserId)
        => _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => string.Equals(user.UserName, userName) || user.ExternalUserId == externalUserId);

    public async Task<Result<CreateUserResult>> CreateUser(UserEntity userEntity)
    {
        var isUserExists = await _dbContext.Users
            .AnyAsync(user => string.Equals(userEntity.UserName, user.UserName));

        if (isUserExists)
            return Result<CreateUserResult>.FromError(UserAccountErrors.UserAlreadyExist.Error);

        _dbContext.Users.Add(userEntity);
        await DbContext.SaveChangesAsync();

        return new Result<CreateUserResult>(new CreateUserResult { UserId = userEntity.Id, IsSuccess = true });
    }

    public async Task<Result> UpdateUser(UserEntity userEntity)
    {
        _dbContext.Users.Update(userEntity);
        await DbContext.SaveChangesAsync();

        return Result.Successful;
    }

    public async Task<Result> ReplenishBalance(Guid userId, decimal replenishmentAmount)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user is null)
            return UserAccountErrors.NotFound;

        user.Money += replenishmentAmount;

        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        return Result.Successful;
    }

    public async Task<Result<UserEntity>> GetSystemUser()
    {
        var systemUser = await DbSet.FirstOrDefaultAsync(x => x.Role.HasFlag(UserRole.System));

        return systemUser is null 
            ? Result<UserEntity>.FromError(UserAccountErrors.NotFound.Error) 
            : Result.FromValue(systemUser);
    }

    public Task<Guid?> GetUserIdByExternalId(Guid externalId)
        => DbSet.Where(x => x.ExternalUserId == externalId)
            .Select(x => x.Id == default ? (Guid?)null : x.Id)
            .FirstOrDefaultAsync();

    public Task<UserEntity> GetUserByExternalId(Guid externalId)
        => DbSet.AsNoTracking()
            .SingleOrDefaultAsync(x => x.ExternalUserId == externalId);

    public override IQueryable<UserEntity> AddIncludes(IQueryable<UserEntity> query)
    {
        return query.Include(x => x.Photo)
            .Include(x => x.BookedTickets)
            .Include(x => x.PurchasedUserTickets);
    }
}