using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Errors;
using Theater.Abstractions.PieceGenre;
using Theater.Common;
using Theater.Contracts.Theater;

namespace Theater.Core.Theater.Validators;

public sealed class PieceGenreValidator : IDocumentValidator<PiecesGenreParameters>
{
    private readonly IPieceGenreRepository _pieceGenreRepository;

    public PieceGenreValidator(IPieceGenreRepository pieceGenreRepository)
    {
        _pieceGenreRepository = pieceGenreRepository;
    }

    public Task<WriteResult> CheckIfCanCreate(PiecesGenreParameters parameters, Guid? userId = null)
    {
        return Task.FromResult(WriteResult.Successful);
    }

    public Task<WriteResult> CheckIfCanUpdate(Guid entityId, PiecesGenreParameters parameters, Guid? userId = null)
    {
        return Task.FromResult(WriteResult.Successful);
    }

    public async Task<WriteResult> CheckIfCanDelete(Guid entityId, Guid? userId = null)
    {
        var hasPieces = await _pieceGenreRepository.HasPieces(entityId);

        return hasPieces 
            ? WriteResult.FromError(PieceGenreErrors.HasPieces.Error)
            :  WriteResult.Successful;
    }
}