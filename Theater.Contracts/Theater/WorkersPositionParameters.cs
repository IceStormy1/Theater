using Theater.Common;

namespace Theater.Contracts.Theater;

public class WorkersPositionParameters
{
    /// <summary>
    /// Наименование роли
    /// </summary>
    public string PositionName { get; set; }

    /// <summary>
    /// Тип должности
    /// </summary>
    public PositionType PositionType { get; set; }
}