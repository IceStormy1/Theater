using System;
using System.Collections.Generic;

namespace Theater.Contracts.Theater.PiecesTicket
{
    public sealed class PieceTicketBuyRequest
    {
        /// <summary>
        /// Идентификаторы билетов для бронирования/покупки
        /// </summary>
        public IReadOnlyCollection<Guid> TicketIds { get; set; }
    }
}
