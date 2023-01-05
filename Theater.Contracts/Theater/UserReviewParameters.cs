using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
