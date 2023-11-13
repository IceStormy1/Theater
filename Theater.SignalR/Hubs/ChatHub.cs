using System.Security.Claims;
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
    private readonly ChatManager _chatManager;

    public ChatHub(
        ILogger<ChatHub> logger,
        IRoomRepository roomsRepository,
        ChatManager chatManager)
    {
        _logger = logger;
        _roomsRepository = roomsRepository;
        _chatManager = chatManager;
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

        _chatManager.ConnectUser(AuthorizedUserId, Context.ConnectionId);

        await UpdateUsersAsync();

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        if (AuthorizedUserId != Guid.Empty)
        {
            var rooms = await _roomsRepository.GetRoomsForUser(AuthorizedUserId, new RoomSearchSettings());
            foreach (var room in rooms)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.Id.ToString());
            }
        }

        _chatManager.DisconnectUser(Context.ConnectionId);

        await UpdateUsersAsync();

        await base.OnDisconnectedAsync(exception);
    }

    public async Task UpdateUsersAsync()
    {
        var users = _chatManager.Users.Select(x => x.UserId).ToList();
        await Clients.All.UpdateUsersAsync(users);
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