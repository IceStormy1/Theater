namespace Theater.SignalR.Hubs;

public class ChatUser
{
    public ChatUser(Guid userId)
    {
        UserId = userId;
        Connections = new List<ChatConnection>();
    }

    /// <summary>
    /// User identity name
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// UTC time connected
    /// </summary>
    public DateTime? ConnectedAt
    {
        get
        {
            if (Connections.Any())
            {
                return Connections
                    .OrderByDescending(x => x.ConnectedAt)
                    .Select(x => x.ConnectedAt)
                    .First();
            }

            return null;
        }
    }

    /// <summary>
    /// All user connections
    /// </summary>
    public List<ChatConnection> Connections { get; }

    /// <summary>
    /// Append connection for user
    /// </summary>
    /// <param name="connectionId"></param>
    public void AppendConnection(string connectionId)
    {
        if (connectionId == null)
        {
            throw new ArgumentNullException(nameof(connectionId));
        }

        var connection = new ChatConnection
        {
            ConnectedAt = DateTime.UtcNow,
            ConnectionId = connectionId
        };

        Connections.Add(connection);
    }

    /// <summary>
    /// Remove connection from user
    /// </summary>
    public void RemoveConnection(string connectionId)
    {
        if (connectionId == null)
            throw new ArgumentNullException(nameof(connectionId));

        var connection = Connections.SingleOrDefault(x => x.ConnectionId.Equals(connectionId));
        if (connection == null)
            return;
        
        Connections.Remove(connection);
    }
}