using System.ComponentModel.DataAnnotations;

namespace Theater.Common;

/// <summary>
/// Тип должности
/// </summary>
public enum PositionType
{
    /// <summary>
    /// Режиссер
    /// </summary>
    [Display(Name = "Режиссер")]
    Producer = 1,

    /// <summary>
    /// Актер
    /// </summary>
    [Display(Name = "Актер")]
    Actor,

    /// <summary>
    /// Художник
    /// </summary>
    [Display(Name = "Художник")]
    Artist,

    /// <summary>
    /// Музыкант
    /// </summary>
    [Display(Name = "Музыкант")]
    Musician
}