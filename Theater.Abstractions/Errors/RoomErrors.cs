using Theater.Common;

namespace Theater.Abstractions.Errors;

public static class RoomErrors
{
    /// <summary>
    /// Указанная запись не найдена
    /// </summary>
    public static Result RoomNotFoundError =>
        Result.FromError(ErrorModel.NotFound("not-found", "Указанный чат не найден"));
}