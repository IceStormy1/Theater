using Theater.Common.Enums;

namespace Theater.Contracts.Rooms;

public class RoomParameters
{
    /// <summary>
    /// Название чата
    /// </summary>
    public string Title { get; set; }

    /// <inheritdoc cref="RoomType"/>
    public RoomType Type { get; set; }
}