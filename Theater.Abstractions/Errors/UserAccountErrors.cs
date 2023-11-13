using Theater.Common;

namespace Theater.Abstractions.Errors;

public static class UserAccountErrors
{
    /// <summary>
    /// Пользователь не найден в системе
    /// </summary>
    public static Result NotFound =>
        Result.FromError(ErrorModel.NotFound("account/not-found", "Пользователь не найден в системе"));

    /// <summary>
    /// Для выполнения операции необходимо авторизоваться
    /// </summary>
    public static Result Unauthorized =>
        Result.FromError(ErrorModel.Unauthorized("account/not-found", "Для выполнения операции необходимо авторизоваться"));

    /// <summary>
    /// Указанный пользователь уже существует
    /// </summary>
    public static Result UserAlreadyExist =>
        Result.FromError(ErrorModel.Default("account/user-already-exist", "Указанный пользователь уже существует"));

    /// <summary>
    /// На балансе недостаточно средств
    /// </summary>
    public static Result NotEnoughMoney =>
        Result.FromError(ErrorModel.Default("account/not-enough-money", "На балансе недостаточно средств"));

    /// <summary>
    /// Недостаточно прав для совершения операции
    /// </summary>
    public static Result InsufficientRights
        => Result.FromError(ErrorModel.Forbidden("account/insufficient-rights ", "Недостаточно прав для совершения операции"));
}