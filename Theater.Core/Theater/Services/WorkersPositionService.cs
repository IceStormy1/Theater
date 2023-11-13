using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.WorkersPosition;
using Theater.Common.Enums;
using Theater.Contracts.Theater.WorkersPosition;
using Theater.Entities.Theater;

namespace Theater.Core.Theater.Services;

public sealed class WorkersPositionService : BaseCrudService<WorkersPositionParameters, WorkersPositionEntity>, IWorkersPositionService
{
    private readonly IWorkersPositionRepository _workersPositionRepository;

    public WorkersPositionService(
        IMapper mapper,
        IWorkersPositionRepository workersPositionRepository,
        IDocumentValidator<WorkersPositionParameters> documentValidator,
        ILogger<WorkersPositionService> logger) : base(mapper, workersPositionRepository, documentValidator, logger)
    {
        _workersPositionRepository = workersPositionRepository;
    }

    public async Task<IReadOnlyCollection<WorkersPositionModel>> GetWorkerPositions(PositionType? positionType)
    {
        var positionsEntities = await _workersPositionRepository.GetWorkerPositions(positionType);

        return Mapper.Map<IReadOnlyCollection<WorkersPositionModel>>(positionsEntities);
    }
}