﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions.Authorization;
using Theater.Contracts.Authorization;
using Theater.Entities.Authorization;

namespace Theater.Core.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IMapper _mapper;
        private readonly IJwtHelper _jwtHelper;

        public AuthorizationService(
            IAuthorizationRepository authorizationRepository,
            IMapper mapper,
            IJwtHelper jwtHelper)
        {
            _authorizationRepository = authorizationRepository;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
        }

        public async Task<UserModel> GetUserById(Guid userId)
        {
            var user = await _authorizationRepository.GetUserById(userId);

            return _mapper.Map<UserModel>(user);
        }

        public async Task<IList<UserModel>> GetUsers()
        {
            var users = await _authorizationRepository.GetUsers();

            return _mapper.Map<List<UserModel>>(users);
        }

        public async Task<(bool IsSuccess, Guid? UserId)> CreateUser(UserParameters user)
        {
            var userEntity = _mapper.Map<UserEntity>(user);

            return await _authorizationRepository.CreateUser(userEntity);
        }

        public async Task<AuthenticateResponse> Authorize(AuthenticateParameters authenticateParameters)
        {
            var userEntity = await _authorizationRepository
                .FindUser(authenticateParameters.UserName, authenticateParameters.Password);

            if (userEntity == null)
                return null;

            var token = _jwtHelper.GenerateJwtToken(userEntity);

            return new AuthenticateResponse { Token = token };
        }
    }
}
