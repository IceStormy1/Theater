using System;
using System.Collections.Generic;

namespace Theater.Contracts.Theater
{
    public class PieceTicketCreateParameters
    {
        /// <summary>
        /// Идентификатор даты начала пьесы
        /// </summary>
        public Guid PieceDateId { get; set; }

        /// <summary>
        /// Билеты пьесы
        /// </summary>
        public IReadOnlyCollection<PiecesTicketParameters> PiecesTickets { get; set; }
    }
}
