using Theater.Common;

namespace Theater.Abstractions.Errors
{
    public static class PieceWorkersErrors
    {
        /// <summary>
        /// Данный работник уже участвует в пьесе
        /// </summary>
        public static WriteResult AlreadyAttached =>
            WriteResult.FromError(ErrorModel.NotFound("piece-worker/already-attached", "Данный работник уже участвует в пьесе"));

        /// <summary>
        /// Запись не найдена
        /// </summary>
        public static WriteResult NotFound =>
            WriteResult.FromError(ErrorModel.NotFound("piece-worker/not-found", "Запись не найдена"));

        /// <summary>
        /// Запись не найдена
        /// </summary>
        public static WriteResult InvalidOperation =>
            WriteResult.FromError(ErrorModel.NotFound("piece-worker/not-found", "Нельзя менять работника театра. Необходимо удалить запись с данным работником и добавить новую"));
    }
}
