using System;

namespace Theater.Contracts.Theater
{
    public class BookedTicketParameters
    {
        /// <summary>
        /// Дата и время бронирования
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Идентификатор билета 
        /// </summary>
        public Guid PiecesTicketId { get; set; }
    }
}
