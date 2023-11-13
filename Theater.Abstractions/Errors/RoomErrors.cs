using Theater.Common;

namespace Theater.Abstractions.Errors;

public static class RoomErrors
{
    /// <summary>
    /// Указанный чат не найден
    /// </summary>
    public static Result RoomNotFoundError =>
        Result.FromError(ErrorModel.NotFound("not-found", "Указанный чат не найден"));

    /// <summary>
    /// В личных сообщениях максимальное количество участников не должно быть больше двух
    /// </summary>
    public static Result InvalidTotalMembersError =>
        Result.FromError(ErrorModel.Default("not-valid", "В личных сообщениях максимальное количество участников не должно быть больше двух"));
}