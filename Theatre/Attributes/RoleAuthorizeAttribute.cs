using Microsoft.AspNetCore.Authorization;
using System;
using Theater.Common.Constants;

namespace Theater.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RoleAuthorizeAttribute : AuthorizeAttribute
{
    public RoleAuthorizeAttribute(string roles) : base(policy: AuthConstants.AuthenticationScheme)
    {
        Roles = roles;
    }
}