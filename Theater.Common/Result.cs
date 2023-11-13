using System.Collections.Generic;

namespace Theater.Common;

public interface IResult
{
    /// <summary>
    /// True, если результат успешный.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Данные ошибки, если результат неуспешный
    /// </summary>
    ErrorModel Error { get; }
}

public interface IResult<out T> : IResult
{
    T ResultData { get; }
}

public sealed class Result<T> : IResult<T>
{
    public Result(T resultData)
    {
        ResultData = resultData;
        IsSuccess = true;
    }

    private Result(ErrorModel error)
    {
        Error = error;
        IsSuccess = false;
    }

    public bool IsSuccess { get; }
    public T ResultData { get; }
    public ErrorModel Error { get; }

    /// <summary>
    /// Возвращает неуспешный результат, исходя из ошибки 
    /// </summary>
    /// <param name="error">Модель ошибки</param>
    public static Result<T> FromError(ErrorModel error) => new(error);
}

public sealed class Result : IResult
{
    /// <summary>
    /// Успешный результат
    /// </summary>
    public static readonly Result Successful = new();

    private Result()
    {
        IsSuccess = true;
    }

    private Result(ErrorModel error)
    {
        Error = error;
        IsSuccess = false;
    }

    public bool IsSuccess { get; }
    public ErrorModel Error { get; }

    /// <summary>
    /// Возвращает успешный результат, исходя из полученной модели 
    /// </summary>
    /// <param name="value">Модель</param>
    public static Result<T> FromValue<T>(T value) => new(value);

    /// <summary>
    /// Возвращает неуспешный результат, исходя из ошибки 
    /// </summary>
    /// <param name="error">Модель ошибки</param>
    public static Result FromError(ErrorModel error) => new(error);
}

public class ErrorModel
{
    private ErrorModel(string type, string message, ErrorKind errorKind)
    {
        Type = type;
        Message = message;
        Kind = errorKind;
    }

    /// <summary>
    /// Error key/type
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Тип ошибки
    /// </summary>
    public ErrorKind Kind { get; }

    public IReadOnlyDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>(0);

    /// <summary>
    /// Возвращает дефолтную модель ошибки
    /// </summary>
    /// <param name="type">Error key</param>
    /// <param name="message">Error message</param>
    public static ErrorModel Default(string type, string message) => new(type, message, ErrorKind.Default);

    /// <summary>
    /// Возвращает модель ошибки, если нет прав доступа 
    /// </summary>
    /// <param name="type">Error key</param>
    /// <param name="message">Error message</param>
    public static ErrorModel Forbidden(string type, string message) =>
        new(type, message, ErrorKind.Forbidden);

    /// <summary>
    /// Возвращает модель ошибки, если сущность не была найдена
    /// </summary>
    /// <param name="type">Error key</param>
    /// <param name="message">Error message</param>
    public static ErrorModel NotFound(string type, string message) =>
        new(type, message, ErrorKind.NotFound);

    /// <summary>
    /// Возвращает модель ошибки, если пользователь не авторизован
    /// </summary>
    /// <param name="type">Error key</param>
    /// <param name="message">Error message</param>
    public static ErrorModel Unauthorized(string type, string message) =>
        new(type, message, ErrorKind.Unauthorized);
}

public enum ErrorKind
{
    /// <summary>
    /// Дефолтный
    /// </summary>
    Default,

    /// <summary>
    /// Нет доступа
    /// </summary>
    Forbidden,

    /// <summary>
    /// Не найдено
    /// </summary>
    NotFound,

    /// <summary>
    /// Не авторизован
    /// </summary>
    Unauthorized
}