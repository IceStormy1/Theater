using Theater.Common;

namespace Theater.Abstractions.Errors;

public static class PieceGenreErrors
{
    /// <summary>
    /// Нельзя удалять жанры для которых создана пьеса
    /// </summary>
    public static Result HasPieces =>
        Result.FromError(ErrorModel.NotFound("genre/has-relationships", "Нельзя удалять жанры для которых создана пьеса"));
}