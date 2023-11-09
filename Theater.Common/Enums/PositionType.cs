using System.ComponentModel.DataAnnotations;

namespace Theater.Common.Enums;

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
    Actor = 2,

    /// <summary>
    /// Художник
    /// </summary>
    [Display(Name = "Художник")]
    Artist = 3,

    /// <summary>
    /// Музыкант
    /// </summary>
    [Display(Name = "Музыкант")]
    Musician = 4
}