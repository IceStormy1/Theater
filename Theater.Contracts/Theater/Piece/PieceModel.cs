using System;
using System.Collections.Generic;
using Theater.Contracts.FileStorage;
using Theater.Contracts.Theater.UserReview;

namespace Theater.Contracts.Theater.Piece;

public sealed class PieceModel : PieceShortInformationModel
{
    /// <summary>
    /// Описание пьесы
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Мета-данные доп.изображений 
    /// </summary>
    public IReadOnlyCollection<StorageFileListItem> AdditionalPhotos { get; set; } = Array.Empty<StorageFileListItem>();

    /// <summary>
    /// Отзывы пользователей
    /// </summary>
    public IReadOnlyCollection<UserReviewModel> UserReviews { get; set; } = new List<UserReviewModel>();
}