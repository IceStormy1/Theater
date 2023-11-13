using System;
using Theater.Common.Enums;
using Theater.Entities.Theater;
using Theater.Entities.Users;

namespace Theater.Entities.FileStorage;

public sealed class FileStorageEntity : BaseEntity, IHasCreatedAt
{
    /// <summary>
    /// Исходное наименование файла
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Наименование файла в хранилище
    /// </summary>
    public string FileStorageName { get; set; }

    /// <summary>
    /// Идентификатор бакета
    /// </summary>
    public BucketIdentifier BucketId { get; set; }

    /// <summary>
    /// Тип файла
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Размер файла
    /// </summary>
    public ulong Size { get; set; }

    /// <inheritdoc cref="IHasCreatedAt.CreatedAt"/>
    public DateTime CreatedAt { get; set; }

    public PieceEntity Piece { get; set; }
    public UserEntity User { get; set; }
}