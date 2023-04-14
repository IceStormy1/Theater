using System;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Errors;
using Theater.Abstractions.PieceDates;
using Theater.Common;
using Theater.Contracts.Theater;

namespace Theater.Core.Theater.Validators
{
    public sealed class PiecesDateValidator : IDocumentValidator<PieceDateParameters>
    {
        private readonly IPieceRepository _pieceRepository;

        public PiecesDateValidator(IPieceRepository pieceRepository)
        {
            _pieceRepository = pieceRepository;
        }

        public async Task<WriteResult> CheckIfCanCreate(PieceDateParameters parameters, Guid? userId = null)
        {
            return await CheckIfCanCreateOrUpdate(parameters);
        }

        public async Task<WriteResult> CheckIfCanUpdate(Guid entityId, PieceDateParameters parameters, Guid? userId = null)
        {
            return await CheckIfCanCreateOrUpdate(parameters);
        }

        public Task<WriteResult> CheckIfCanDelete(Guid entityId, Guid? userId = null)
        {
            return Task.FromResult(WriteResult.Successful);
        }

        private async Task<WriteResult> CheckIfCanCreateOrUpdate(PieceDateParameters parameters)
        {
            var pieceEntity = await _pieceRepository.GetPieceWithDates(parameters.PieceId);

            if (pieceEntity is null)
                return WriteResult.FromError(PieceErrors.NotFound.Error);

            return pieceEntity.PieceDates.Any(x => x.Date.Date == parameters.Date.Date) 
                ? WriteResult.FromError(PieceErrors.DateAlreadyExists.Error)
                : WriteResult.Successful;
        }
    }
}
