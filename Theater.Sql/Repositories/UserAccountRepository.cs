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

    public async Task<UserEntity> FindUser(string userName, string password, int? vkId = null)
    {
        var userQuery = _dbContext.Users.AsNoTracking().AsQueryable();

        if (vkId != null)
            return await userQuery.FirstOrDefaultAsync(user => user.VkId == vkId);

        return await userQuery.FirstOrDefaultAsync(user => string.Equals(password, user.Password)
                                                         && (string.Equals(user.UserName, userName)
                                                             || string.Equals(user.Email, userName)));
    }

        public async Task<Result<CreateUserResult>> CreateUser(UserEntity userEntity)
    {
        var isUserExists = await _dbContext.Users
            .AnyAsync(user => string.Equals(userEntity.UserName, user.UserName) ||
                              string.Equals(userEntity.Email, user.Email));

        if (isUserExists)
            return Result<CreateUserResult>.FromError(UserAccountErrors.UserAlreadyExist.Error);

        userEntity.UserRole = await _dbContext.UserRoles.FirstOrDefaultAsync(x => x.Id == userEntity.RoleId); // todo: переделать, нужно для авторизации ВК 
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
        var systemUser = await DbSet.FirstOrDefaultAsync(x => x.RoleId == (int)UserRole.System);

        return systemUser is null 
            ? Result<UserEntity>.FromError(UserAccountErrors.NotFound.Error) 
            : Result.FromValue(systemUser);
    }

    public override IQueryable<UserEntity> AddIncludes(IQueryable<UserEntity> query)
    {
        return query.Include(x => x.Photo)
            .Include(x => x.BookedTickets)
            .Include(x => x.PurchasedUserTickets);
    }
}