using System.ComponentModel.DataAnnotations;
using System;

namespace Theater.Contracts.FileStorage;

/// <summary>
/// Модель записи с информацией о файле в хранилище S3 для списочного представления
/// </summary>
public class StorageFileListItem
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя файла
    /// </summary>
    [Required]
    public string FileName { get; set; }

    /// <summary>
    /// Размер файла
    /// </summary>
    public long? Size { get; set; }

    /// <summary>
    /// Дата и время загрузки файла
    /// </summary>
    public DateTime UploadAt { get; set; }

    /// <summary>
    /// URL файла
    /// </summary>
    public string DirectUrl { get; set; }
}