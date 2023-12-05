using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Theater.Contracts;

public sealed class Page<T>
{
    /// <summary>
    /// Список записей
    /// </summary>
    [Required]
    public IReadOnlyCollection<T> Items { get; set; } = Array.Empty<T>();
    /// <summary>
    /// Общее количество записей
    /// </summary>
    public int Total { get; set; }
}