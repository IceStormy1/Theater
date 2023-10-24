using System.Security.Claims;
using Theater.Abstractions.Filters;
using Theater.Abstractions.Rooms;
using Theater.Abstractions.UserAccount;
using HubConnectionContext = Microsoft.AspNetCore.SignalR.HubConnectionContext;
using IUserIdProvider = Microsoft.AspNetCore.SignalR.IUserIdProvider;

namespace Theater.SignalR;

public class ChatHub : AuthorizedHub<IChatClient>
{
    public const string Url = "/hubs";
    private readonly ILogger<ChatHub> _logger;
    private readonly IRoomsRepository _roomsRepository;

    public ChatHub(
        ILogger<ChatHub> logger,
        IRoomsRepository roomsRepository,
        IUserAccountRepository usersRepository) : base(usersRepository)
    {
        _logger = logger;
        _roomsRepository = roomsRepository;
    }

    public async Task EnterRoom(Guid roomId)
    {
        var room = await _roomsRepository.GetActiveRoomRelationForUser(AuthorizedUserExternalId, roomId);
        if (room == null)
        {
            _logger.LogWarning("Комната {roomId} не найдена", roomId);
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, room.Room.Id.ToString());
    }

    public async Task ExitRoom(Guid roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
    }

    public override async Task OnConnectedAsync()
    {
        if (AuthorizedUserExternalId != Guid.Empty)
        {
            var rooms = await _roomsRepository.GetRoomsForUser(AuthorizedUserExternalId, new RoomSearchSettings());
            foreach (var room in rooms)
                await Groups.AddToGroupAsync(Context.ConnectionId, room.Id.ToString());
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (AuthorizedUserExternalId != Guid.Empty)
        {
            var rooms = await _roomsRepository.GetRoomsForUser(AuthorizedUserExternalId, new RoomSearchSettings());
            foreach (var room in rooms)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.Id.ToString());
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}

public class UserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        var userId = connection.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        return Guid.TryParse(userId, out var userIdResult) ? userIdResult.ToString() : null;
    }
}