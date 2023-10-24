using Theater.Common;

namespace Theater.Abstractions.Errors;

public static class TheaterWorkerErrors
{
    /// <summary>
    /// Пьеса не найден в системе
    /// </summary>
    public static Result NotFound =>
        Result.FromError(ErrorModel.NotFound("theater-worker/not-found", "Работник театра не найден в системе"));
}