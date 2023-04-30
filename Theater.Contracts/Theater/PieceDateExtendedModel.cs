using System.Collections.Generic;

namespace Theater.Contracts.Theater;

public sealed class PieceDateExtendedModel : PieceDateModel
{
    /// <summary>
    /// Билеты пьесы
    /// </summary>
    public IReadOnlyCollection<PiecesTicketModel> PiecesTickets { get; set; }
}