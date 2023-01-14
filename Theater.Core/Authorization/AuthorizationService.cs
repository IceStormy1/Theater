using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions.Authorization;
using Theater.Abstractions.Authorization.Models;
using Theater.Common;
using Theater.Contracts.Authorization;
using Theater.Entities.Authorization;

namespace Theater.Core.Authorization
{
    public class AuthorizationService : ServiceBase<IAuthorizationRepository>, IAuthorizationService
    {
        private readonly IJwtHelper _jwtHelper;

        public AuthorizationService(
            IMapper mapper,
            IAuthorizationRepository repository,
            IJwtHelper jwtHelper) : base(mapper, repository)
        {
            _jwtHelper = jwtHelper;
        }

        public async Task<UserModel> GetUserById(Guid userId)
        {
            var user = await Repository.GetUserById(userId);

            return Mapper.Map<UserModel>(user);
        }

        public async Task<IList<UserModel>> GetUsers()
        {
            var users = await Repository.GetUsers();

            return Mapper.Map<List<UserModel>>(users);
        }

        public async Task<WriteResult<CreateUserResult>> CreateUser(UserParameters user)
        {
            var userEntity = Mapper.Map<UserEntity>(user);

            return await Repository.CreateUser(userEntity);
        }

        public async Task<AuthenticateResponse> Authorize(AuthenticateParameters authenticateParameters)
        {
            var userEntity = await Repository
                .FindUser(authenticateParameters.UserName, authenticateParameters.Password);

            if (userEntity == null)
                return null;

            var token = _jwtHelper.GenerateJwtToken(userEntity);

            return new AuthenticateResponse { Token = token };
        }
    }
}
