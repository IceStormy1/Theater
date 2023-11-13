using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Authorization;
using Theater.Abstractions.Authorization.Models;
using Theater.Abstractions.Errors;
using Theater.Abstractions.UserAccount;
using Theater.Common;
using Theater.Contracts.Authorization;
using Theater.Contracts.UserAccount;
using Theater.Entities.Users;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace Theater.Core.UserAccount;

public sealed class UserAccountService : BaseCrudService<UserParameters, UserEntity>, IUserAccountService
{
    private const string SystemUser = "SystemUser";

    private readonly IJwtHelper _jwtHelper;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IVkApi _vkApiAuth;

    public UserAccountService(
        IMapper mapper, 
        IUserAccountRepository repository,
        IDocumentValidator<UserParameters> documentValidator,
        IJwtHelper jwtHelper,
        ILogger<UserAccountService> logger,
        IVkApi vkApiAuth) : base(mapper, repository, documentValidator, logger)
    {
        _jwtHelper = jwtHelper;
        _vkApiAuth = vkApiAuth;
        _userAccountRepository = repository;
    }

    public async Task<Result<CreateUserResult>> CreateUser(UserParameters user)
    {
        var userEntity = Mapper.Map<UserEntity>(user);

        return await _userAccountRepository.CreateUser(userEntity);
    }

    public async Task<Result> UpdateUser(UserParameters user, Guid userId)
    {
        var userEntity = await Repository.GetByEntityId(userId);

        if(userEntity is null)
            return Result.FromError(UserAccountErrors.NotFound.Error);

        Mapper.Map(user, userEntity);

        return await _userAccountRepository.UpdateUser(userEntity);
    }

    public async Task<AuthenticateResponse> Authorize(AuthenticateParameters authenticateParameters)
    {
        var userEntity = await _userAccountRepository.FindUser(authenticateParameters.UserName, authenticateParameters.Password);

        return userEntity == null 
            ? null 
            : GetAuthenticateResponseByUser(userEntity);
    }

    public async Task<Result<AuthenticateResponse>> AuthorizeWithVk(AuthenticateVkDto authenticateVkDto)
    {
        var @params = new ApiAuthParams { Settings = Settings.All, AccessToken = authenticateVkDto.AccessToken };
        try
        {
            await _vkApiAuth.AuthorizeAsync(@params);

            var vkUserInfo = await _vkApiAuth.Account.GetProfileInfoAsync();

            if (vkUserInfo is null)
                return Result<AuthenticateResponse>.FromError(UserAccountErrors.NotFound.Error);

            var userEntity = await _userAccountRepository.FindUser(userName:null, password: null, vkId: default); // todo: Добавить нормальный vkID

            if (userEntity is null)
            {
                userEntity = Mapper.Map<UserEntity>(vkUserInfo);

                var createResult = await _userAccountRepository.CreateUser(userEntity);

                if (!createResult.IsSuccess)
                    return Result<AuthenticateResponse>.FromError(createResult.Error);
            }

            var authenticateResponse = GetAuthenticateResponseByUser(userEntity);

            return Result.FromValue(authenticateResponse);

        }
        catch (Exception e)
        {
            Logger.LogError(e, "Произошла ошибка во время авторизации VK");

            return Result<AuthenticateResponse>.FromError(AbstractionErrors.InternalError.Error);
        }
    }

    public Task<Result> ReplenishBalance(Guid userId, decimal replenishmentAmount)
        => _userAccountRepository.ReplenishBalance(userId, replenishmentAmount);

    private AuthenticateResponse GetAuthenticateResponseByUser(UserEntity userEntity)
    {
        var token = _jwtHelper.GenerateJwtToken(userEntity);

        return new AuthenticateResponse { AccessToken = token, Id = userEntity.Id };
    }
}