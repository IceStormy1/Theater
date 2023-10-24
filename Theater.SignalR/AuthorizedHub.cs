using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Theater.Abstractions.UserAccount;

namespace Theater.SignalR;

[Authorize]
public abstract class AuthorizedHub<T> : Hub<T> where T : class
{
    private readonly IUserAccountRepository _usersRepository;

    protected AuthorizedHub(IUserAccountRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    protected Guid AuthorizedUserExternalId
    {
        get
        {
            var userId = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(userId, out var userIdResult) ? userIdResult : Guid.Empty;
        }
    }
}