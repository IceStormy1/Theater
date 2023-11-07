using Theater.Common;

namespace Theater.Abstractions.Errors;

public static class AbstractionErrors
{
    /// <summary>
    /// Произошла непредвиденная ошибка
    /// </summary>
    public static Result InternalError =>
        Result.FromError(ErrorModel.NotFound("internal-error", "Произошла непредвиденная ошибка"));

    /// <summary>
    /// Указанная запись не найдена
    /// </summary>
    public static Result NotFoundError =>
        Result.FromError(ErrorModel.NotFound("not-found", "Указанная запись не найдена"));
}