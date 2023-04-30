using AutoMapper;
using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Authorization;
using Theater.Abstractions.Authorization.Models;
using Theater.Abstractions.Errors;
using Theater.Abstractions.FileStorage;
using Theater.Abstractions.UserAccount;
using Theater.Common;
using Theater.Contracts.Authorization;
using Theater.Contracts.UserAccount;
using Theater.Entities.Authorization;

namespace Theater.Core.UserAccount;

public sealed class UserAccountService : ServiceBase<UserParameters, UserEntity>, IUserAccountService
{
    private readonly IJwtHelper _jwtHelper;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IFileStorageService _fileStorageService;

    public UserAccountService(
        IMapper mapper, 
        IUserAccountRepository repository,
        IDocumentValidator<UserParameters> documentValidator,
        IJwtHelper jwtHelper, 
        IFileStorageService fileStorageService) : base(mapper, repository, documentValidator)
    {
        _jwtHelper = jwtHelper;
        _fileStorageService = fileStorageService;
        _userAccountRepository = repository;
    }

    public async Task<WriteResult<CreateUserResult>> CreateUser(UserParameters user)
    {
        // TODO: Валидация на уникальность пользователя в системе
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
        var userEntity = await _userAccountRepository
            .FindUser(authenticateParameters.UserName, authenticateParameters.Password);

        if (userEntity == null)
            return null;

        var token = _jwtHelper.GenerateJwtToken(userEntity);

        return new AuthenticateResponse { AccessToken = token, Id = userEntity.Id };
    }

    public async Task<WriteResult> ReplenishBalance(Guid userId, decimal replenishmentAmount)
    {
        return await _userAccountRepository.ReplenishBalance(userId, replenishmentAmount);
    }
}