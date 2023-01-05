using System;

namespace Theater.Contracts.Authorization
{
    public class UserParameters : UserBase
    {
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
        /// Дата рождения
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Идентификатор фотографии пользователя в ЛК 
        /// </summary>
        public Guid? PhotoId { get; set; }
    }
}
