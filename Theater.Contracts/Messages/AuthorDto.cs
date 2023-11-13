using System;
using Theater.Contracts.FileStorage;

namespace Theater.Contracts.Messages;

public sealed class AuthorDto
{
    /// <summary>
    /// Идентификатор автора сообщения
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Автор сообщения (имя пользователя)
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Мета-данные фотографии пользователя в ЛК 
    /// </summary>
    public StorageFileListItem Photo { get; set; }
}