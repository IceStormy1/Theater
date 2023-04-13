using AutoMapper;
using Theater.Abstractions;
using Theater.Abstractions.PieceWorkers;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Core.Theater.PieceWorkers
{
    internal class PieceWorkersService : ServiceBase<PieceWorkerParameters, PieceWorkerEntity>, IPieceWorkersService
    {
        public PieceWorkersService(
            IMapper mapper,
            ICrudRepository<PieceWorkerEntity> repository,
            IDocumentValidator<PieceWorkerParameters> documentValidator) : base(mapper, repository, documentValidator)
        {
        }
    }
}
