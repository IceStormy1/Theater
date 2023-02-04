using System;

namespace Theater.Entities.Theater
{
    public sealed class PieceWorkerEntity
    {
        /// <summary>
        /// Идентификатор 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор работника театра 
        /// </summary>
        public Guid TheaterWorkerId { get; set; }

        /// <summary>
        /// Ссылка на работника театра
        /// </summary>
        public TheaterWorkerEntity TheaterWorker { get; set; }

        /// <summary>
        /// Идентификатор пьесы
        /// </summary>
        public Guid PieceId { get; set; }

        /// <summary>
        /// Ссылка на пьесу
        /// </summary>
        public PieceEntity Piece { get; set; }
    }
}