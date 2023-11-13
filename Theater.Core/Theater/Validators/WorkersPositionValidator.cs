using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Errors;
using Theater.Abstractions.WorkersPosition;
using Theater.Common;
using Theater.Contracts.Theater.WorkersPosition;

namespace Theater.Core.Theater.Validators;

public sealed class WorkersPositionValidator : IDocumentValidator<WorkersPositionParameters>
{
    private readonly IWorkersPositionRepository _workersPositionRepository;

    public WorkersPositionValidator(IWorkersPositionRepository workersPositionRepository)
    {
        _workersPositionRepository = workersPositionRepository;
    }

    public Task<Result> CheckIfCanCreate(WorkersPositionParameters parameters, Guid? userId = null)
    {
        return Task.FromResult(Result.Successful);
    }

    public Task<Result> CheckIfCanUpdate(Guid entityId, WorkersPositionParameters parameters, Guid? userId = null)
    {
        return Task.FromResult(Result.Successful);
    }

    public async Task<Result> CheckIfCanDelete(Guid entityId, Guid? userId = null)
    {
        var hasTheaterWorkers = await _workersPositionRepository.HasTheaterWorkers(entityId);

        return !hasTheaterWorkers 
            ? Result.Successful 
            : Result.FromError(WorkersPositionErrors.HasTheaterWorkers.Error);
    }
}