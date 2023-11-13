using System;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Errors;
using Theater.Abstractions.PieceDates;
using Theater.Common;
using Theater.Contracts.Theater.PieceDate;

namespace Theater.Core.Theater.Validators;

public sealed class PiecesDateValidator : IDocumentValidator<PieceDateParameters>
{
    private readonly IPieceRepository _pieceRepository;

    public PiecesDateValidator(IPieceRepository pieceRepository)
    {
        _pieceRepository = pieceRepository;
    }

    public async Task<Result> CheckIfCanCreate(PieceDateParameters parameters, Guid? userId = null)
    {
        return await CheckIfCanCreateOrUpdate(parameters);
    }

    public async Task<Result> CheckIfCanUpdate(Guid entityId, PieceDateParameters parameters, Guid? userId = null)
    {
        return await CheckIfCanCreateOrUpdate(parameters);
    }

    public Task<Result> CheckIfCanDelete(Guid entityId, Guid? userId = null)
    {
        return Task.FromResult(Result.Successful);
    }

    private async Task<Result> CheckIfCanCreateOrUpdate(PieceDateParameters parameters)
    {
        var pieceEntity = await _pieceRepository.GetPieceWithDates(parameters.PieceId);

        if (pieceEntity is null)
            return Result.FromError(PieceErrors.NotFound.Error);

        return pieceEntity.PieceDates.Any(x => x.Date.Date == parameters.Date.Date) 
            ? Result.FromError(PieceErrors.DateAlreadyExists.Error)
            : Result.Successful;
    }
}