using System;

namespace Theater.Common.Enums;

/// <summary>
/// Роль пользователя
/// </summary>
[Flags]
public enum UserRole
{
    /// <summary>
    /// Неизвестное значение
    /// </summary>
    None = 0,

    /// <summary>
    /// Обычный пользователь
    /// </summary>
    User = 1,

    /// <summary>
    /// Админ
    /// </summary>
    Admin = 2,

    /// <summary>
    /// Системный пользователь
    /// </summary>
    System = 4
}