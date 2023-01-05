using System.Collections.Generic;

namespace Theater.Entities.Theater
{
    public class PiecesGenreEntity
    {
        /// <summary>
        /// Идентификатор роли
        /// </summary>
        public ushort Id { get; set; }

        /// <summary>
        /// Наименование роли
        /// </summary>
        public string GenreName { get; set; }

        public List<PieceEntity> Pieces { get; set; }
    }
}
