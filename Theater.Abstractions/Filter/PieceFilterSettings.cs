namespace Theater.Abstractions.Filter
{
    public class PieceFilterSettings : PagingSortSettings
    {
        /// <summary>
        /// Идентификатор жанра
        /// </summary>
        public int? GenreId { get; set; }
    }
}
