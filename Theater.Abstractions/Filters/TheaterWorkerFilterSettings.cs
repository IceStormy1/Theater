using Theater.Common.Enums;

namespace Theater.Abstractions.Filters;

public sealed class TheaterWorkerFilterSettings : PagingSortSettings
{
    /// <summary>
    /// Идентификатор должности работника
    /// </summary>
    public PositionType? PositionTypeId { get; set; }
}