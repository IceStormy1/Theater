using System;

namespace Theater.Entities;

public abstract class BaseEntity : IEntity
{
    /// <inheritdoc cref="IEntity.Id"/>
    public Guid Id { get; set; }
}