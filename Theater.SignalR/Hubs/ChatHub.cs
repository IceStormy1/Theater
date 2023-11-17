using System.Security.Claims;
using Theater.Abstractions.Caches;
using Theater.Abstractions.Filters;
using Theater.Abstractions.Rooms;
using HubConnectionContext = Microsoft.AspNetCore.SignalR.HubConnectionContext;
using IUserIdProvider = Microsoft.AspNetCore.SignalR.IUserIdProvider;

namespace Theater.SignalR.Hubs;

public sealed class ChatHub : AuthorizedHub<IChatClient>
{
    public const string Url = "/hubs";
    private readonly ILogger<ChatHub> _logger;
    private readonly IRoomRepository _roomsRepository;
    private readonly IConnectionsCache _connectionsCache;

    public ChatHub(
        ILogger<ChatHub> logger,
        IRoomRepository roomsRepository,
        IConnectionsCache connectionsCache)
    {
        _logger = logger;
        _roomsRepository = roomsRepository;
        _connectionsCache = connectionsCache;
    }

    public async Task EnterRoom(Guid roomId)
    {
        var room = await _roomsRepository.GetActiveRoomRelationForUser(AuthorizedUserId, roomId);
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
        if (AuthorizedUserId != Guid.Empty)
        {
            var rooms = await _roomsRepository.GetRoomsForUser(AuthorizedUserId, new RoomSearchSettings());
            foreach (var room in rooms)
                await Groups.AddToGroupAsync(Context.ConnectionId, room.Id.ToString());
        }

        await base.OnConnectedAsync();
        await _connectionsCache.SetConnection(AuthorizedUserId, Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await _connectionsCache.RemoveConnection(AuthorizedUserId, Context.ConnectionId);

        if (AuthorizedUserId != Guid.Empty)
        {
            var rooms = await _roomsRepository.GetRoomsForUser(AuthorizedUserId, new RoomSearchSettings());
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