using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Errors;
using Theater.Abstractions.PieceWorkers;
using Theater.Common;
using Theater.Contracts.Theater.PieceWorker;

namespace Theater.Core.Theater.Validators;

public class PieceWorkersValidator : IDocumentValidator<PieceWorkerParameters>
{
    private readonly IPieceWorkersRepository _pieceWorkersRepository;

    public PieceWorkersValidator(IPieceWorkersRepository pieceWorkersRepository)
    {
        _pieceWorkersRepository = pieceWorkersRepository;
    }

    public async Task<WriteResult> CheckIfCanCreate(PieceWorkerParameters parameters, Guid? userId = null)
    {
        var alreadyAttached = await _pieceWorkersRepository.CheckWorkerRelation(parameters.TheaterWorkerId, parameters.PieceId);

        return alreadyAttached 
            ? WriteResult.FromError(PieceWorkersErrors.AlreadyAttached.Error) 
            : WriteResult.Successful;
    }

    public async Task<WriteResult> CheckIfCanUpdate(Guid entityId, PieceWorkerParameters parameters, Guid? userId = null)
    {
        var pieceWorkerEntity = await _pieceWorkersRepository.GetByEntityId(entityId);

        if (pieceWorkerEntity is null)
            return WriteResult.FromError(PieceWorkersErrors.NotFound.Error);

        return pieceWorkerEntity.TheaterWorkerId != parameters.TheaterWorkerId 
            ? WriteResult.FromError(PieceWorkersErrors.InvalidOperation.Error) 
            : WriteResult.Successful;
    }

    public Task<WriteResult> CheckIfCanDelete(Guid entityId, Guid? userId = null)
    {
        return Task.FromResult(WriteResult.Successful);
    }
}