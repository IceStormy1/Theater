using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Abstractions.PieceWorkers;

public interface IPieceWorkersService : ICrudService<PieceWorkerParameters, PieceWorkerEntity>
{
}