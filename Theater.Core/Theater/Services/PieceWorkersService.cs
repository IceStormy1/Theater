using AutoMapper;
using Microsoft.Extensions.Logging;
using Theater.Abstractions;
using Theater.Abstractions.PieceWorkers;
using Theater.Contracts.Theater.PieceWorker;
using Theater.Entities.Theater;

namespace Theater.Core.Theater.Services;

internal class PieceWorkersService : BaseCrudService<PieceWorkerParameters, PieceWorkerEntity>, IPieceWorkersService
{
    public PieceWorkersService(
        IMapper mapper,
        ICrudRepository<PieceWorkerEntity> repository,
        IDocumentValidator<PieceWorkerParameters> documentValidator,
        ILogger<PieceWorkersService> logger) : base(mapper, repository, documentValidator, logger)
    {
    }
}