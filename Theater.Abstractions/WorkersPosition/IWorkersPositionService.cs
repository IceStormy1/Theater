using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Abstractions.WorkersPosition;

public interface IWorkersPositionService : ICrudService<WorkersPositionParameters, WorkersPositionEntity>
{
}