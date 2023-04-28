using Theater.Common;

namespace Theater.Abstractions.Filter;

public sealed class TheaterWorkerFilterSettings : PagingSortSettings
{
    /// <summary>
    /// Идентификатор должности работника
    /// </summary>
    public PositionType? PositionTypeId { get; set; }
}