using System;

namespace Theater.Contracts.Theater.PiecesTicket;

public sealed class PiecesTicketModel : PiecesTicketParameters
{
    /// <summary>
    /// Идентификатор билета
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// true - билет забронирован/куплен
    /// false - билет доступен для бронирования/покупки
    /// </summary>
    public bool IsBooked { get; set; }
}