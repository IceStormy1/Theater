using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace Theater.Configuration.Policy;

internal static class AuthorizationOptionsExtension
{
    /// <summary>
    /// Формирует политики на основе массива ролей из конфигурации
    /// </summary>
    public static void AddRoleModelPolicies<T>(this AuthorizationOptions options, IConfiguration configuration, string section) where T : class
    {
        var policies = configuration.GetSection($"RoleModel:{section}:Policies").Get<T>();

        var policiesProp = typeof(T).GetProperties().ToList();
        foreach (var prop in policiesProp)
        {
            var roles = (string[])prop.GetValue(policies);
            if (roles is null || roles.Length == 0)
                continue;

            options.AddPolicy(prop.Name, policy => policy.RequireRole(roles));
        }
    }
}