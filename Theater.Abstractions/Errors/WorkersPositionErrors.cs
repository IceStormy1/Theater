using Theater.Common;

namespace Theater.Abstractions.Errors;

public static class WorkersPositionErrors
{
    /// <summary>
    /// Невозможно удалить запись, т.к есть связанные работники театра
    /// </summary>
    public static Result HasTheaterWorkers =>
        Result.FromError(ErrorModel.NotFound("position/has-relation", "Невозможно удалить запись, т.к есть связанные работники театра"));
}