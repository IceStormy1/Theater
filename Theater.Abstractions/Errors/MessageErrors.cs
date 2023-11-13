using Theater.Common;

namespace Theater.Abstractions.Errors;

public static class MessageErrors
{
    /// <summary>
    /// Сообщение не найдено
    /// </summary>
    public static Result MessageNotFoundError =>
        Result.FromError(ErrorModel.NotFound("not-found", "Сообщение не найдено"));
}