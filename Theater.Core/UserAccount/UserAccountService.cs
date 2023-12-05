using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Caches;
using Theater.Abstractions.Errors;
using Theater.Abstractions.UserAccount;
using Theater.Common;
using Theater.Contracts.UserAccount;
using Theater.Entities.Users;

namespace Theater.Core.UserAccount;

public sealed class UserAccountService : BaseCrudService<UserParameters, UserEntity>, IUserAccountService
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IUserAccountCache _userAccountCache;

    public UserAccountService(
        IMapper mapper, 
        IUserAccountRepository repository,
        IDocumentValidator<UserParameters> documentValidator,
        ILogger<UserAccountService> logger, 
        IUserAccountCache userAccountCache
        ) : base(mapper, repository, documentValidator, logger)
    {
        _userAccountRepository = repository;
        _userAccountCache = userAccountCache;
    }

    public async Task<Result> UpdateUserProfile(UserParameters user, Guid userId)
    {
        var userEntity = await Repository.GetByEntityId(userId);

        if(userEntity is null)
            return Result.FromError(UserAccountErrors.NotFound.Error);

        Mapper.Map(user, userEntity);

        return await _userAccountRepository.UpdateUser(userEntity);
    }

    public async Task<Result<UserModel>> CreateOrUpdateUser(ClaimsPrincipal userClaims)
    {
        var userModel = Mapper.Map<UserModel>(userClaims.Claims);

        var user = await _userAccountRepository.FindUser(userModel.UserName, userModel.ExternalUserId);

        return user is null 
            ? await CreateUser(userModel) 
            : await UpdateUser(userModel, user);
    }

    public Task<Result> ReplenishBalance(Guid userId, decimal replenishmentAmount)
        => _userAccountRepository.ReplenishBalance(userId, replenishmentAmount);

    public async Task<Guid?> GetUserIdByExternalId(Guid externalId)
    {
        var innerUserId = await _userAccountCache.GetInnerUserIdByExternalId(externalId);

        if (innerUserId.HasValue)
            return innerUserId;

        innerUserId = await _userAccountRepository.GetUserIdByExternalId(externalId);

        if(!innerUserId.HasValue)
            return null;

        await _userAccountCache.LinkUserIds(externalId, innerUserId.Value);
        
        return innerUserId;
    }

    private async Task<Result<UserModel>> CreateUser(UserModel userModel)
    {
        var userEntity = Mapper.Map<UserEntity>(userModel);
        var createResult = await _userAccountRepository.CreateUser(userEntity);

        if (!createResult.IsSuccess)
            return Result<UserModel>.FromError(createResult.Error);

        userModel.Id = userEntity.Id;

        return Result.FromValue(userModel);
    }

    private async Task<Result<UserModel>> UpdateUser(UserModel userModel, UserEntity userEntity)
    {
        Mapper.Map(userModel, userEntity);

        var updateResult = await _userAccountRepository.UpdateUser(userEntity);

        if (!updateResult.IsSuccess)
            return Result<UserModel>.FromError(updateResult.Error);

        userModel.Id = userEntity.Id;

        return Result.FromValue(userModel);
    }
}