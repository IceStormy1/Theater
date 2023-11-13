using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Theater.SignalR.Hubs;

[Authorize]
public abstract class AuthorizedHub<T> : Hub<T> where T : class
{
    protected Guid AuthorizedUserId
    {
        get
        {
            var userId = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(userId, out var userIdResult) ? userIdResult : Guid.Empty;
        }
    }
}