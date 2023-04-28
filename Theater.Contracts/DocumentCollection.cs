using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Theater.Contracts;

/// <summary>
/// Содержит список элементов. 
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class DocumentCollection<T>
{
    /// <summary>
    /// Список документов.
    /// </summary>
    [Required]
    [MinLength(0)]
    public IReadOnlyCollection<T> Items { get; }

    public DocumentCollection(IReadOnlyCollection<T> items) => Items = items;
}