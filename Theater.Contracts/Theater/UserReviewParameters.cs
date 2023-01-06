using System;

namespace Theater.Contracts.Theater
{
    public class UserReviewParameters
    {
        /// <summary>
        /// Текст рецензии
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Заголовок рецензии
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Идентификатор пьесы
        /// </summary>
        public Guid PieceId { get; set; }
    }
}
