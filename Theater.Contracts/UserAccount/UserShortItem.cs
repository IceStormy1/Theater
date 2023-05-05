using System;

namespace Theater.Contracts.UserAccount
{
    public class UserShortItem
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

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
        /// ФИО
        /// </summary>
        public string FullName => $"{LastName} {FirstName} {MiddleName}";

        /// <summary>
        /// Телефон пользователя
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime BirthDate { get; set; }
    }
}
