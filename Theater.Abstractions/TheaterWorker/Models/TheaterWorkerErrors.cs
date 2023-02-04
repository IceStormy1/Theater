using Theater.Common;

namespace Theater.Abstractions.TheaterWorker.Models
{
    public static class TheaterWorkerErrors
    {
        /// <summary>
        /// Пьеса не найден в системе
        /// </summary>
        public static WriteResult NotFound =>
            WriteResult.FromError(ErrorModel.NotFound("theater-worker/not-found", "Работник театра не найден в системе"));
    }
}
