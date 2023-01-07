using System;
using System.Collections.Generic;

namespace Theater.Abstractions.Piece.Models
{
    public class PieceDto : PieceShortInformationDto
    {
        /// <summary>
        /// Описание пьесы
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Краткое описание пьесы
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Идентификаторы изображений 
        /// </summary>
        public IReadOnlyCollection<Guid> PhotoIds { get; set; } = new List<Guid>();
    }
}