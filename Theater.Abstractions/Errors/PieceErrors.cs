using Theater.Common;

namespace Theater.Abstractions.Errors;

public static class PieceErrors
{
    /// <summary>
    /// Пьеса не найден в системе
    /// </summary>
    public static Result NotFound =>
        Result.FromError(ErrorModel.NotFound("piece/not-found", "Пьеса не найдена в системе"));

    /// <summary>
    /// Дата для указанной пьесы уже существует
    /// </summary>
    public static Result DateAlreadyExists =>
        Result.FromError(ErrorModel.NotFound("piece/not-found", "Дата для указанной пьесы уже существует"));
}