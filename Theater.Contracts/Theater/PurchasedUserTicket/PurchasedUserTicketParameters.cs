using System;

namespace Theater.Contracts.Theater.PurchasedUserTicket;

public class PurchasedUserTicketParameters
{
    /// <summary>
    /// Дата покупки 
    /// </summary>
    public DateTime DateOfPurchase { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор билета
    /// </summary>
    public Guid TicketPriceEventsId { get; set; }

    /// <summary>
    /// Версия билета
    /// </summary>
    public int TicketPriceEventsVersion { get; set; }
}