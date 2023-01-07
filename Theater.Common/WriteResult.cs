using System.Collections.Generic;

namespace Theater.Common
{
    public interface IWriteResult
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

    public interface IWriteResult<out T> : IWriteResult
    {
        T ResultData { get; }
    }

    public sealed class WriteResult<T> : IWriteResult<T>
    {
        public WriteResult(T resultData)
        {
            ResultData = resultData;
            IsSuccess = true;
        }

        private WriteResult(ErrorModel error)
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
        public static WriteResult<T> FromError(ErrorModel error) => new(error);
    }

    public class WriteResult : IWriteResult
    {
        /// <summary>
        /// Успешный результат
        /// </summary>
        public static readonly WriteResult Successful = new();

        private WriteResult()
        {
            IsSuccess = true;
        }

        private WriteResult(ErrorModel error)
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
        public static WriteResult<T> FromValue<T>(T value) => new(value);

        /// <summary>
        /// Возвращает неуспешный результат, исходя из ошибки 
        /// </summary>
        /// <param name="error">Модель ошибки</param>
        public static WriteResult FromError(ErrorModel error) => new(error);
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

        /// <summary>
        /// Вспомогательный метод для конвертации ошибки в <see cref="WriteResult"/> 
        /// </summary>
        public WriteResult ToWriteResult() => WriteResult.FromError(this);

        /// <summary>
        /// Вспомогательный метод для конвертации ошибки в <see cref="WriteResult"/>
        /// </summary>
        public WriteResult<T> ToWriteResult<T>() => WriteResult<T>.FromError(this);
    }

    public enum ErrorKind
    {
        /// <summary>
        /// Дефольтный
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
}
