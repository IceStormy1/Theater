using System;
using System.Collections.Generic;
using Theater.Entities.FileStorage;

namespace Theater.Entities.Theater;

public sealed class PieceEntity : BaseEntity, IHasCreatedAt, IHasUpdatedAt
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
    /// Ссылка на жанр пьесы
    /// </summary>
    public PiecesGenreEntity Genre { get; set; }

    /// <summary>
    /// Основная фотография пьесы
    /// </summary>
    public FileStorageEntity MainPhoto { get; set; }
    
    /// <summary>
    /// Идентификатор основной фотографии
    /// </summary>
    public Guid MainPhotoId { get; set; }

    /// <summary>
    /// Дополнительные фотографии пьесы
    /// </summary>
    public List<Guid> PhotoIds { get; set; }

    /// <inheritdoc cref="IHasCreatedAt.CreatedAt"/>
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc cref="IHasUpdatedAt.UpdatedAt"/>
    public DateTime? UpdatedAt { get; set; }

    public List<UserReviewEntity> UserReviews { get; set; }
    public List<PieceDateEntity> PieceDates { get; set; }
    public List<PieceWorkerEntity> PieceWorkers { get; set; }
}