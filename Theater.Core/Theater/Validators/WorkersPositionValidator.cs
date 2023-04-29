using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Errors;
using Theater.Abstractions.WorkersPosition;
using Theater.Common;
using Theater.Contracts.Theater;

namespace Theater.Core.Theater.Validators;

public sealed class WorkersPositionValidator : IDocumentValidator<WorkersPositionParameters>
{
    private readonly IWorkersPositionRepository _workersPositionRepository;

    public WorkersPositionValidator(IWorkersPositionRepository workersPositionRepository)
    {
        _workersPositionRepository = workersPositionRepository;
    }

    public Task<WriteResult> CheckIfCanCreate(WorkersPositionParameters parameters, Guid? userId = null)
    {
        return Task.FromResult(WriteResult.Successful);
    }

    public Task<WriteResult> CheckIfCanUpdate(Guid entityId, WorkersPositionParameters parameters, Guid? userId = null)
    {
        return Task.FromResult(WriteResult.Successful);
    }

    public async Task<WriteResult> CheckIfCanDelete(Guid entityId, Guid? userId = null)
    {
        var hasTheaterWorkers = await _workersPositionRepository.HasTheaterWorkers(entityId);

        return !hasTheaterWorkers 
            ? WriteResult.Successful 
            : WriteResult.FromError(WorkersPositionErrors.HasTheaterWorkers.Error);
    }
}