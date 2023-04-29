using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Theater.Abstractions;
using Theater.Abstractions.WorkersPosition;
using Theater.Common;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Core.Theater;

public sealed class WorkersPositionService : ServiceBase<WorkersPositionParameters, WorkersPositionEntity>, IWorkersPositionService
{
    private readonly IWorkersPositionRepository _workersPositionRepository;

    public WorkersPositionService(
        IMapper mapper,
        IWorkersPositionRepository workersPositionRepository, 
        IDocumentValidator<WorkersPositionParameters> documentValidator) : base(mapper, workersPositionRepository, documentValidator)
    {
        _workersPositionRepository = workersPositionRepository;
    }

    public async Task<IReadOnlyCollection<WorkersPositionModel>> GetWorkerPositions(PositionType? positionType)
    {
        var positionsEntities = await _workersPositionRepository.GetWorkerPositions(positionType);

        return Mapper.Map<IReadOnlyCollection<WorkersPositionModel>>(positionsEntities);
    }
}