namespace Theater.SignalR.Hubs;

/// <summary>
/// User connection form one of the device 
/// </summary>
public class ChatConnection
{
    /// <summary>
    /// Дата и время подключения к чату
    /// </summary>
    public DateTime ConnectedAt { get; set; }

    /// <summary>
    /// Connection Id клиента
    /// </summary>
    public string ConnectionId { get; set; } = null!;
}