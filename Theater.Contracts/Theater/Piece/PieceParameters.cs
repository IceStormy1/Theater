using System;
using System.Collections.Generic;
using Theater.Contracts.FileStorage;

namespace Theater.Contracts.Theater.Piece;

public class PieceParameters
{
    /// <summary>
    /// Наименование пьесы
    /// </summary>
    public string PieceName { get; set; }

    /// <summary>
    /// Описание пьесы
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Краткое описание пьесы
    /// </summary>
    public string ShortDescription { get; set; }

    /// <summary>
    /// Идентификатор жанра пьесы
    /// </summary>
    public Guid GenreId { get; set; }

    /// <summary>
    /// Мета-данные основного изображения пьесы
    /// </summary>
    public StorageFileListItem MainPhoto { get; set; }

    /// <summary>
    /// Мета-данные доп.изображений 
    /// </summary>
    public IReadOnlyCollection<StorageFileListItem> AdditionalPhotos { get; set; } = Array.Empty<StorageFileListItem>();
}