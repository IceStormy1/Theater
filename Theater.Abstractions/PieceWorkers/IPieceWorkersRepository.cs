using System.Threading.Tasks;
using System;
using Theater.Entities.Theater;

namespace Theater.Abstractions.PieceWorkers
{
    public interface IPieceWorkersRepository : ICrudRepository<PieceWorkerEntity>
    {
        Task<bool> CheckWorkerRelation(Guid theaterWorkerId, Guid pieceId);
    }
}
