using System;
using System.Collections.Generic;
using Theater.Common.Enums;
using Theater.Entities.FileStorage;
using Theater.Entities.Rooms;
using Theater.Entities.Theater;

namespace Theater.Entities.Users;

/// <summary>
/// Пользователь
/// </summary>
public sealed class UserEntity : BaseEntity, IHasCreatedAt, IHasUpdatedAt
{
    /// <summary>
    /// Идентификатор пользователя, который авторизовался при помощи VK
    /// </summary>
    public int? VkId { get; set; }

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

    /// <inheritdoc cref="IHasCreatedAt.CreatedAt"/>
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc cref="IHasUpdatedAt.UpdatedAt"/>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Идентификатор фотографии пользователя в ЛК 
    /// </summary>
    public Guid? PhotoId { get; set; }

    /// <summary>
    /// Основная фотография пьесы
    /// </summary>
    public FileStorageEntity Photo { get; set; }

    /// <summary>
    /// Ссылка на роль пользователя
    /// </summary>
    public UserRoleEntity UserRole { get; set; }

    public List<UserReviewEntity> UserReviews { get; set; } = new();
    public List<BookedTicketEntity> BookedTickets { get; set; } = new();
    public List<PurchasedUserTicketEntity> PurchasedUserTickets { get; set; } = new();

    /// <summary>
    /// Чаты пользователя
    /// </summary>
    public List<UserRoomEntity> UserRooms { get; set; } = new();

    public List<MessageEntity> Messages { get; set; } = new();
}