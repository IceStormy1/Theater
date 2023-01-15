using System;

namespace Theater.Abstractions.Piece.Models
{
    public class PieceDateDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата и время пьесы
        /// </summary>
        public DateTime Date { get; set; }
    }
}
