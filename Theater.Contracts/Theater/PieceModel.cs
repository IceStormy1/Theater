using System;
using System.Collections.Generic;
using Theater.Contracts.FileStorage;

namespace Theater.Contracts.Theater;

public sealed class PieceModel : PieceShortInformationModel
{
    /// <summary>
    /// Описание пьесы
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Краткое описание пьесы
    /// </summary>
    public string ShortDescription { get; set; }

    /// <summary>
    /// Мета-данные доп.изображений 
    /// </summary>
    public IReadOnlyCollection<StorageFileListItem> AdditionalPhotos { get; set; } = Array.Empty<StorageFileListItem>();

    /// <summary>
    /// Отзывы пользователей
    /// </summary>
    public IReadOnlyCollection<UserReviewModel> UserReviews { get; set; } = new List<UserReviewModel>();
}