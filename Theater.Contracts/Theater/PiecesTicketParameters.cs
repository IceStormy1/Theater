using System;

namespace Theater.Contracts.Theater
{
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
        /// Идентификатор даты начала пьесы
        /// </summary>
        public Guid PieceDateId { get; set; }
    }
}
