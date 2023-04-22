using System;

namespace Theater.Contracts.Theater
{
    public sealed class UserReviewModel : UserReviewParameters
    {
        /// <summary>
        /// Идентификатор работника театра
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }
    }
}