namespace Theater.Contracts.Filters;

public class PieceFilterParameters : PagingSortParameters
{
    /// <summary>
    /// Идентификатор жанра
    /// </summary>
    public int? GenreId { get; set; }
}