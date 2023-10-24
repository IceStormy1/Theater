using Theater.Common;

namespace Theater.Abstractions.Errors;

public static class AbstractionErrors
{
    /// <summary>
    /// Дата для указанной пьесы уже существует
    /// </summary>
    public static Result InternalError =>
        Result.FromError(ErrorModel.NotFound("internal-error", "Произошла непредвиденная ошибка"));
}