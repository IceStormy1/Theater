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
using Theater.Entities.Authorization;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace Theater.Core.UserAccount;

public sealed class UserAccountService : ServiceBase<UserParameters, UserEntity>, IUserAccountService
{
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

    public async Task<WriteResult<CreateUserResult>> CreateUser(UserParameters user)
    {
        var userEntity = Mapper.Map<UserEntity>(user);

        return await _userAccountRepository.CreateUser(userEntity);
    }

    public async Task<WriteResult> UpdateUser(UserParameters user, Guid userId)
    {
        var userEntity = await Repository.GetByEntityId(userId);

        if(userEntity is null)
            return WriteResult.FromError(UserAccountErrors.NotFound.Error);

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

    public async Task<WriteResult<AuthenticateResponse>> AuthorizeWithVk(AuthenticateVkDto authenticateVkDto)
    {
        var @params = new ApiAuthParams { Settings = Settings.All, AccessToken = authenticateVkDto.AccessToken };
        try
        {
            await _vkApiAuth.AuthorizeAsync(@params);

            var vkUserInfo = await _vkApiAuth.Account.GetProfileInfoAsync();

            if (vkUserInfo is null)
                return WriteResult<AuthenticateResponse>.FromError(UserAccountErrors.NotFound.Error);

            var userEntity = await _userAccountRepository.FindUser(userName:null, password: null, vkId: vkUserInfo.Id);

            if (userEntity is null)
            {
                userEntity = Mapper.Map<UserEntity>(vkUserInfo);

                var createResult = await _userAccountRepository.CreateUser(userEntity);

                if (!createResult.IsSuccess)
                    return WriteResult<AuthenticateResponse>.FromError(createResult.Error);
            }

            var authenticateResponse = GetAuthenticateResponseByUser(userEntity);

            return WriteResult.FromValue(authenticateResponse);

        }
        catch (Exception e)
        {
            Logger.LogError(e, "Произошла ошибка во время авторизации VK");

            return WriteResult<AuthenticateResponse>.FromError(AbstractionErrors.InternalError.Error);
        }
    }

    public async Task<WriteResult> ReplenishBalance(Guid userId, decimal replenishmentAmount)
    {
        return await _userAccountRepository.ReplenishBalance(userId, replenishmentAmount);
    }

    private AuthenticateResponse GetAuthenticateResponseByUser(UserEntity userEntity)
    {
        var token = _jwtHelper.GenerateJwtToken(userEntity);

        return new AuthenticateResponse { AccessToken = token, Id = userEntity.Id };
    }
}