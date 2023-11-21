﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Theater.Abstractions.Authorization;
using Theater.Abstractions.Jwt;
using Theater.Common.Extensions;
using Theater.Entities.Users;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Theater.Core.Authorization;

public sealed class JwtHelper : IJwtHelper
{
    private static JwtOptions _jwtOptions;

    public JwtHelper(IOptions<JwtOptions> jwOptions)
    {
        _jwtOptions = jwOptions.Value;
    }

    public string GenerateJwtToken(UserEntity user)
    {
        var securityKey = _jwtOptions.GetSymmetricSecurityKey();
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.Email, user.Email),
            new (JwtRegisteredClaimNames.PhoneNumber, user.Phone),
            new (JwtRegisteredClaimNames.Name, user.FullName),
            new (JwtRegisteredClaimNames.Birthdate, user.BirthDate.ToString("dd/MM/yyyy")),
            new (JwtRegisteredClaimNames.Gender, user.Gender.ToString("D")),
            new ("role", user.UserRole.RoleName.ToLower()),
            new (nameof(UserEntity.UserName).ToCamelCase(), user.UserName)
        };

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims, 
            expires: DateTime.UtcNow.AddSeconds(_jwtOptions.TokenLifetime),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}