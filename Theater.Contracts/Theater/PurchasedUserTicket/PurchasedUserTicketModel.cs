using System;

namespace Theater.Contracts.Theater.PurchasedUserTicket
{
    public sealed class PurchasedUserTicketModel : PurchasedUserTicketParameters
    {
        /// <summary>
        /// Идентификаотр
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор пьесы
        /// </summary>
        public Guid PieceId { get; set; }

        /// <summary>
        /// Наименование пьесы
        /// </summary>
        public string PieceName { get; set; }

        /// <summary>
        /// Идентификатор даты пьесы
        /// </summary>
        public Guid PieceDateId { get; set; }

        /// <summary>
        /// Дата пьесы
        /// </summary>
        public DateTime PieceDate { get; set; }
    }
}
