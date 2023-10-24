using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Theater.Entities.Theater;

public sealed class PieceDateEntity : BaseEntity
{
    /// <summary>
    /// Дата и время пьесы
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Идентификатор пьесы
    /// </summary>
    public Guid PieceId { get; set; }

    /// <summary>
    /// Ссылка на пьесу
    /// </summary>
    [JsonIgnore]
    public PieceEntity Piece { get; set; }

    [JsonIgnore]
    public List<PiecesTicketEntity> PiecesTickets { get; set; }
}