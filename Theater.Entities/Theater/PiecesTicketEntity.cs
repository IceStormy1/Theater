using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Theater.Entities.Theater;

public sealed class PiecesTicketEntity : BaseEntity
{
    /// <summary>
    /// Ряд
    /// </summary>
    public ushort TicketRow { get; set; }

    /// <summary>
    /// Место
    /// </summary>
    public ushort TicketPlace { get; set; }
        
    /// <summary>
    /// Стоимость билета
    /// </summary>
    public ushort TicketPrice { get; set; }

    /// <summary>
    /// Идентификатор даты начала пьесы
    /// </summary>
    public Guid PieceDateId { get; set; }

    /// <summary>
    /// Ссылка на дату начала пьесы
    /// </summary>
    public PieceDateEntity PieceDate { get; set; }

    public BookedTicketEntity BookedTicket { get; set; }

    [JsonIgnore]
    public List<TicketPriceEventsEntity> TicketPriceEvents { get; set; }
}