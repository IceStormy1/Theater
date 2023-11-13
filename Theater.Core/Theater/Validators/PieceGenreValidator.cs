using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Errors;
using Theater.Abstractions.PieceGenre;
using Theater.Common;
using Theater.Contracts.Theater.PiecesGenre;

namespace Theater.Core.Theater.Validators;

public sealed class PieceGenreValidator : IDocumentValidator<PiecesGenreParameters>
{
    private readonly IPieceGenreRepository _pieceGenreRepository;

    public PieceGenreValidator(IPieceGenreRepository pieceGenreRepository)
    {
        _pieceGenreRepository = pieceGenreRepository;
    }

    public Task<Result> CheckIfCanCreate(PiecesGenreParameters parameters, Guid? userId = null)
    {
        return Task.FromResult(Result.Successful);
    }

    public Task<Result> CheckIfCanUpdate(Guid entityId, PiecesGenreParameters parameters, Guid? userId = null)
    {
        return Task.FromResult(Result.Successful);
    }

    public async Task<Result> CheckIfCanDelete(Guid entityId, Guid? userId = null)
    {
        var hasPieces = await _pieceGenreRepository.HasPieces(entityId);

        return hasPieces 
            ? Result.FromError(PieceGenreErrors.HasPieces.Error)
            :  Result.Successful;
    }
}