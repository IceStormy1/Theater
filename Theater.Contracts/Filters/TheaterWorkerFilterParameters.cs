using Theater.Common;

namespace Theater.Contracts.Filters;

public sealed class TheaterWorkerFilterParameters : PagingSortParameters
{
    /// <summary>
    /// Идентификатор должности работника
    /// </summary>
    public PositionType? PositionTypeId { get; set; }
}