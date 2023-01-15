using System;

namespace Theater.Contracts.Theater
{
    public class PiecesTicketModel : PiecesTicketParameters
    {
        /// <summary>
        /// Идентификатор билета
        /// </summary>
        public Guid Id { get; set; }
    }
}
