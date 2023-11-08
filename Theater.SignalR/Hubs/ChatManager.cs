namespace Theater.SignalR.Hubs;

public sealed class ChatManager
{
    public List<ChatUser> Users { get; } = new();// TODO: переделать на редис

    public void ConnectUser(Guid userId, string connectionId)
    {
        var alreadyExistingUser = GetConnectedUserById(userId);

        if(alreadyExistingUser is null)
        {
            alreadyExistingUser = new ChatUser(userId);
            Users.Add(alreadyExistingUser);
        }

        alreadyExistingUser.AppendConnection(connectionId);
    }

    /// <summary>
    /// Disconnect user from connection.
    /// If we found the connection is last, than we remove user from user list.
    /// </summary>
    /// <param name="connectionId"></param>
    public bool DisconnectUser(string connectionId)
    {
        var existingUser = GetConnectedUserByConnectionId(connectionId);
        if (existingUser == null)
            return false;
        
        if (!existingUser.Connections.Any())
            return false; // should never happen, but...

        var connectionExists = existingUser.Connections.Any(x => x.ConnectionId == connectionId);//.Select(x => x.ConnectionId).First().Equals(connectionId);
        if (!connectionExists)
            return false; // should never happen, but...

        if (existingUser.Connections.Count() == 1)
        {
            Users.Remove(existingUser);
            return true;
        }

        existingUser.RemoveConnection(connectionId);

        return false;
    }

    /// <summary>
    /// Returns <see cref="ChatUser"/> by connectionId if connection found
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns></returns>
    private ChatUser GetConnectedUserByConnectionId(string connectionId)
        => Users.FirstOrDefault(x => x.Connections.Select(c => c.ConnectionId).Contains(connectionId));

    /// <summary>
    /// Returns <see cref="ChatUser"/> by userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    private ChatUser GetConnectedUserById(Guid userId)
        => Users.FirstOrDefault(x => x.UserId == userId);
}