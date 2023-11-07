using AutoMapper;
using Microsoft.Extensions.Logging;
using Theater.Abstractions;
using Theater.Abstractions.PieceDates;
using Theater.Contracts.Theater.PieceDate;
using Theater.Entities.Theater;

namespace Theater.Core.Theater.Services;

public class PieceDateService : BaseCrudService<PieceDateParameters, PieceDateEntity>, IPieceDateService
{
    public PieceDateService(
        IMapper mapper,
        ICrudRepository<PieceDateEntity> repository,
        IDocumentValidator<PieceDateParameters> documentValidator,
        ILogger<PieceDateService> logger) : base(mapper, repository, documentValidator, logger)
    {
    }
}