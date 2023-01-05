using System;
using System.Collections.Generic;

namespace Theater.Entities.Theater
{
    public class PiecesTicketEntity
    {
        /// <summary>
        /// Идентификатор билета
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ряд
        /// </summary>
        public ushort TicketRow { get; set; }

        /// <summary>
        /// Место
        /// </summary>
        public ushort TicketPlace { get; set; }

        /// <summary>
        /// Идентификатор репертуара
        /// </summary>
        public Guid RepertoryId { get; set; }

        /// <summary>
        /// Ссылка на репертуар
        /// </summary>
        public RepertoryEntity Repertory { get; set; }

        public List<BookedTicketEntity> BookedTickets { get; set; }
        public List<TicketPriceEventsEntity> TicketPriceEvents { get; set; }
    }
}
