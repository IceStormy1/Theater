using Theater.Common;

namespace Theater.Abstractions.Piece.Models
{
    public static class PieceErrors
    {
        /// <summary>
        /// Пьеса не найден в системе
        /// </summary>
        public static WriteResult NotFound =>
            WriteResult.FromError(ErrorModel.NotFound("piece/not-found", "Пьеса не найдена в системе"));
    }
}
