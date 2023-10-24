using System;
using Theater.Entities.Users;

namespace Theater.Entities.Theater;

public sealed class BookedTicketEntity : BaseEntity
{
    /// <summary>
    /// Дата и время бронирования
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Ссылка на пользователя
    /// </summary>
    public UserEntity User { get; set; }

    /// <summary>
    /// Идентификатор билета 
    /// </summary>
    public Guid PiecesTicketId { get; set; }

    /// <summary>
    /// Ссылка на пьесу
    /// </summary>
    public PiecesTicketEntity PiecesTicket { get; set; }
}