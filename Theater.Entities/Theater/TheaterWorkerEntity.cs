using System;
using System.Collections.Generic;
using Theater.Common.Enums;
using Theater.Entities.Users;

namespace Theater.Entities.Theater;

public sealed class TheaterWorkerEntity : BaseEntity, IHasCreatedAt, IHasUpdatedAt
{
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
    /// Пол
    /// </summary>
    public GenderType Gender { get; set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Описание работника
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Идентификатор фотографии работника
    /// </summary>
    public Guid? PhotoId { get; set; }

    /// <summary>
    /// Идентификатор должности работника театра
    /// </summary>
    public Guid PositionId { get; set; }

    /// <inheritdoc cref="IHasCreatedAt.CreatedAt"/>
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc cref="IHasUpdatedAt.UpdatedAt"/>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Ссылка на должность 
    /// </summary>
    public WorkersPositionEntity Position { get; set; }

    public List<PieceWorkerEntity> PieceWorkers { get; set; }
}