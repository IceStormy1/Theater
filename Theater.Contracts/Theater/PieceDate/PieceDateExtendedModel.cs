using System.Collections.Generic;
using Theater.Contracts.Theater.PiecesTicket;

namespace Theater.Contracts.Theater.PieceDate;

public sealed class PieceDateExtendedModel : PieceDateModel
{
    /// <summary>
    /// Билеты пьесы
    /// </summary>
    public IReadOnlyCollection<PiecesTicketModel> PiecesTickets { get; set; }
}