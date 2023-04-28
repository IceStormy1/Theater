using System.Collections.Generic;

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
    /// Идентификаторы изображений 
    /// </summary>
    public IReadOnlyCollection<string> PhotoUrls { get; set; } = new List<string>();

    /// <summary>
    /// Отзывы пользователей
    /// </summary>
    public IReadOnlyCollection<UserReviewModel> UserReviews { get; set; } = new List<UserReviewModel>();
}