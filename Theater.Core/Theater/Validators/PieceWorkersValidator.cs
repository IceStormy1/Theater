using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Errors;
using Theater.Abstractions.PieceWorkers;
using Theater.Common;
using Theater.Contracts.Theater.PieceWorker;

namespace Theater.Core.Theater.Validators;

public sealed class PieceWorkersValidator : IDocumentValidator<PieceWorkerParameters>
{
    private readonly IPieceWorkersRepository _pieceWorkersRepository;

    public PieceWorkersValidator(IPieceWorkersRepository pieceWorkersRepository)
    {
        _pieceWorkersRepository = pieceWorkersRepository;
    }

    public async Task<Result> CheckIfCanCreate(PieceWorkerParameters parameters, Guid? userId = null)
    {
        var alreadyAttached = await _pieceWorkersRepository.CheckWorkerRelation(parameters.TheaterWorkerId, parameters.PieceId);

        return alreadyAttached 
            ? Result.FromError(PieceWorkersErrors.AlreadyAttached.Error) 
            : Result.Successful;
    }

    public async Task<Result> CheckIfCanUpdate(Guid entityId, PieceWorkerParameters parameters, Guid? userId = null)
    {
        var pieceWorkerEntity = await _pieceWorkersRepository.GetByEntityId(entityId);

        if (pieceWorkerEntity is null)
            return Result.FromError(PieceWorkersErrors.NotFound.Error);

        return pieceWorkerEntity.TheaterWorkerId != parameters.TheaterWorkerId 
            ? Result.FromError(PieceWorkersErrors.InvalidOperation.Error) 
            : Result.Successful;
    }

    public Task<Result> CheckIfCanDelete(Guid entityId, Guid? userId = null)
    {
        return Task.FromResult(Result.Successful);
    }
}