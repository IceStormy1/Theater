using System;
using System.Collections.Generic;
using Theater.Entities.Theater;

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
        /// Дата рождения
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public ushort RoleId { get; set; }

        /// <summary>
        /// Деньги пользователя
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// Идентификатор фотографии пользователя в ЛК 
        /// </summary>
        public Guid? PhotoId { get; set; }

        /// <summary>
        /// Ссылка на роль пользователя
        /// </summary>
        public UserRoleEntity UserRole { get; set; }

        public List<UserReviewEntity> Reviews { get; set; }
        public List<BookedTicketEntity> BookedTickets { get; set; }
        public List<PurchasedUserTicketEntity> PurchasedUserTickets { get; set; }
    }
}