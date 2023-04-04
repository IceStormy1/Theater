using System;
using System.Collections.Generic;

namespace Theater.Entities.Theater
{
    public sealed class PiecesTicketEntity : IEntity
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
        /// Стоимость билета
        /// </summary>
        public ushort TicketPrice { get; set; }

        /// <summary>
        /// Идентификатор даты начала пьесы
        /// </summary>
        public Guid PieceDateId { get; set; }

        /// <summary>
        /// Ссылка на дату начала пьесы
        /// </summary>
        public PieceDateEntity PieceDate { get; set; }

        public BookedTicketEntity BookedTicket { get; set; }
        public List<TicketPriceEventsEntity> TicketPriceEvents { get; set; }
    }
}
