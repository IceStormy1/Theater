using AutoMapper;
using Theater.Abstractions;
using Theater.Abstractions.PieceDates;
using Theater.Contracts.Theater.PieceDate;
using Theater.Entities.Theater;

namespace Theater.Core.Theater.Services;

public class PieceDateService : ServiceBase<PieceDateParameters, PieceDateEntity>, IPieceDateService
{
    public PieceDateService(
        IMapper mapper,
        ICrudRepository<PieceDateEntity> repository,
        IDocumentValidator<PieceDateParameters> documentValidator) : base(mapper, repository, documentValidator)
    {
    }
}