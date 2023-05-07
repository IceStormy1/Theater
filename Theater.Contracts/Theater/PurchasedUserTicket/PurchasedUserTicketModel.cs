using System;

namespace Theater.Contracts.Theater.PurchasedUserTicket;

public sealed class PurchasedUserTicketModel : PurchasedUserTicketParameters
{
    /// <summary>
    /// Идентификаотр
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор пьесы
    /// </summary>
    public Guid PieceId { get; set; }

    /// <summary>
    /// Наименование пьесы
    /// </summary>
    public string PieceName { get; set; }

    /// <summary>
    /// Идентификатор даты пьесы
    /// </summary>
    public Guid PieceDateId { get; set; }

    /// <summary>
    /// Дата пьесы
    /// </summary>
    public DateTime PieceDate { get; set; }

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
}