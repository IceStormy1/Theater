using System;

namespace Theater.Abstractions.Piece.Models
{
    public sealed class PieceDateDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата и время пьесы
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Идентификатор пьесы
        /// </summary>
        public Guid PieceId { get; set; }
    }
}
