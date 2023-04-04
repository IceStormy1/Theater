using System;
using System.Collections.Generic;

namespace Theater.Contracts.Theater
{
    public sealed class PieceDateModel : PieceDateParameters
    {
        /// <summary>
        /// Идентификатор даты пьесы
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Билеты пьесы
        /// </summary>
        public IReadOnlyCollection<PiecesTicketModel> PiecesTickets { get; set; }
    }
}