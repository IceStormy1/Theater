using System;

namespace Theater.Contracts.Theater
{
    public class PieceDateParameters
    {
        // TODO: 1. отказаться от PieceDateParameters и перенести поле Date в PiectParameters
        // TODO: 2. Валидацию из PiecesDateValidator перенести в валидатор для пьесы
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