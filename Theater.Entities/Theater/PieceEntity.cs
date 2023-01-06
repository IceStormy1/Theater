using System;
using System.Collections.Generic;

namespace Theater.Entities.Theater
{
    public class PieceEntity
    {
        /// <summary>
        /// Идентификатор пьесы
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование пьесы
        /// </summary>
        public string PieceName { get; set; }

        /// <summary>
        /// Описание пьесы
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Краткое описание пьесы
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Идентификатор жанра пьесы
        /// </summary>
        public ushort GenreId { get; set; }

        /// <summary>
        /// Идентификаторы изображений 
        /// </summary>
        public Guid[] PhotoIds { get; set; } = Array.Empty<Guid>();

        /// <summary>
        /// Ссылка на жанр пьесы
        /// </summary>
        public PiecesGenreEntity Genre { get; set; }

        public List<UserReviewEntity> Reviews { get; set; }
        public List<PieceDateEntity> PieceDates { get; set; }
        public List<PieceWorkerEntity> PieceWorkers { get; set; }
    }
}
