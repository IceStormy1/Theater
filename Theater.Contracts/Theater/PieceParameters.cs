using System;

namespace Theater.Contracts.Theater
{
    public class PieceParameters
    {//todo: добавить работников театра и возможно исправить модели пьес
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
    }
}
