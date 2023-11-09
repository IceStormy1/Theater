namespace Theater.Common.Constants;

public static class MessageConstants
{
    /// <summary>
    /// Максимальная длина сообщения
    /// </summary>
    public const short MessageMaxLength = 2000;

    /// <summary>
    /// Пользователь присоединился к чату
    /// </summary>
    public const string SystemSignInMessage = "Пользователь {0} добавил {1} в чат";

    /// <summary>
    /// Чат создан
    /// </summary>
    public const string SystemCreateRoom = "Чат создан";
}