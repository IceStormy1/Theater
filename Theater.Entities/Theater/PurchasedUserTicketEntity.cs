using System;
using Theater.Entities.Authorization;

namespace Theater.Entities.Theater
{
    public class PurchasedUserTicketEntity
    {
        /// <summary>
        /// Идентификатор купленного билета
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата покупки 
        /// </summary>
        public DateTime DateOfPurchase { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Ссылка на пользователя
        /// </summary>
        public UserEntity User { get; set; }

        /// <summary>
        /// Идентификатор билета
        /// </summary>
        public Guid TicketPriceEventsId { get; set; }

        /// <summary>
        /// Версия билета
        /// </summary>
        public int TicketPriceEventsVersion { get; set; }

        /// <summary>
        /// Ссылка на билет
        /// </summary>
        public TicketPriceEventsEntity TicketPriceEvents { get; set; }
    }
}
