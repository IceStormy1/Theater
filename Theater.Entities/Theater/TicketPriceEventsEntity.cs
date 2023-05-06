using System;

namespace Theater.Entities.Theater;

public sealed class TicketPriceEventsEntity
{
    /// <summary>
    /// Идентификатор билета
    /// </summary>
    public Guid PiecesTicketId { get; set; }

    /// <summary>
    /// Версия цены билета
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// JSON билета
    /// </summary>
    public PiecesTicketEntity Model { get; set; }

    /// <summary>
    /// Время изменения
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Связь с билетом
    /// </summary>
    public PiecesTicketEntity PiecesTicket { get; set; }

    public PurchasedUserTicketEntity PurchasedUserTicket { get; set; }
}