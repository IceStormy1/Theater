using System;
using Theater.Entities.Authorization;

namespace Theater.Entities.Theater;

public sealed class BookedTicketEntity : IEntity
{
    /// <summary>
    /// Идентификатор забронированного билета
    /// </summary>
    public Guid Id { get; set; }

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