using AutoMapper;
using Theater.Abstractions;
using Theater.Abstractions.Piece;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Core.Theater
{
    internal class PieceDateService : ServiceBase<PieceDateParameters, PieceDateEntity>, IPieceDateService
    {
        public PieceDateService(
            IMapper mapper,
            ICrudRepository<PieceDateEntity> repository,
            IDocumentValidator<PieceDateParameters> documentValidator) : base(mapper, repository, documentValidator)
        {
        }
    }
}
