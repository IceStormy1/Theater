namespace Theater.Contracts.Theater.PiecesTicket;

public class PiecesTicketParameters
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
}