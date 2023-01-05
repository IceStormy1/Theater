using System;
using System.Collections.Generic;

namespace Theater.Entities.Theater
{
    public class RepertoryEntity
    {
        /// <summary>
        /// Идентификатор роли
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата начала репертуара
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания репертуара
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Время
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Идентификатор пьесы
        /// </summary>
        public Guid PieceId { get; set; }

        /// <summary>
        /// Ссылка на пьесу
        /// </summary>
        public PieceEntity Piece { get; set; }

        public List<PiecesTicketEntity> PiecesTickets { get; set; }
        public List<RepertoryWorkerEntity> RepertoriesWorkers { get; set; }
    }
}