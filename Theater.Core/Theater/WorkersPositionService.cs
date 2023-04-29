using AutoMapper;
using Theater.Abstractions;
using Theater.Abstractions.WorkersPosition;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Core.Theater;

internal class WorkersPositionService : ServiceBase<WorkersPositionParameters, WorkersPositionEntity>, IWorkersPositionService
{
    public WorkersPositionService(
        IMapper mapper, 
        ICrudRepository<WorkersPositionEntity> repository, 
        IDocumentValidator<WorkersPositionParameters> documentValidator) : base(mapper, repository, documentValidator)
    {
    }
}