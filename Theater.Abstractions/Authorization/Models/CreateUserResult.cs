using System;

namespace Theater.Abstractions.Authorization.Models
{
    public sealed class CreateUserResult
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid? UserId { get; set; } = null;

        /// <summary>
        /// Успешная регистрация?
        /// </summary>
        public bool IsSuccess { get; set; } = false;
    }
}
