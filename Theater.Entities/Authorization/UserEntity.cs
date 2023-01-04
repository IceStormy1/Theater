using System;

namespace Theater.Entities.Authorization
{
    public class UserEntity
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Никнейм пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Email пользователя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Имя 
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия 
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Телефон пользователя
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        /// Дата создания пользователя
        /// </summary>
        public DateTime DateOfCreate { get; set; }

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public ushort RoleId { get; set; }
    }
}
