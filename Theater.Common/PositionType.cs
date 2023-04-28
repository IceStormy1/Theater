using System.ComponentModel.DataAnnotations;

namespace Theater.Common;

/// <summary>
/// Тип должности
/// </summary>
public enum PositionType
{
    [Display(Name = "Режиссер")]
    Producer = 1,

    [Display(Name = "Актер")]
    Actor,

    [Display(Name = "Художник")]
    Artist,

    [Display(Name = "Музыкант")]
    Musician
}