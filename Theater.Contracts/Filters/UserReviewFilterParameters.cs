using System;

namespace Theater.Contracts.Filters
{
    public sealed class UserReviewFilterParameters : PagingSortParameters
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Идентификатор пьесы
        /// </summary>
        public Guid? PieceId { get; set; }
    }
}
