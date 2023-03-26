using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Piece;
using Theater.Abstractions.Piece.Models;
using Theater.Common;
using Theater.Contracts;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Core.Theater
{
    public sealed class PieceService : ServiceBase<IPieceRepository>, IPieceService
    {
        public PieceService(
            IMapper mapper,
            IPieceRepository repository) : base(mapper, repository)
        {
        }

        public async Task<IReadOnlyCollection<PieceShortInformationModel>> GetPiecesShortInformation()
        {
            var piecesShortInformation = await Repository.GetPiecesShortInformation();

            return Mapper.Map<IReadOnlyCollection<PieceShortInformationModel>>(piecesShortInformation);
        }

        public async Task<WriteResult<PieceModel>> GetPieceById(Guid pieceId)
        {
            var pieceDto = await Repository.GetPieceDtoById(pieceId);

            if(pieceDto is null)
                return WriteResult<PieceModel>.FromError(PieceErrors.NotFound.Error);

            var pieceResult = Mapper.Map<PieceModel>(pieceDto);

            return WriteResult.FromValue(pieceResult);
        }

        public async Task<WriteResult<DocumentMeta>> CreateOrUpdatePiece(PieceParameters parameters, Guid? pieceId)
        {
            var piece = await Repository.GetByEntityId(pieceId ?? Guid.NewGuid());

            if (piece is null)
            {
                piece = Mapper.Map<PieceEntity>(parameters);

                await Repository.Add(piece);
            }

            Mapper.Map(parameters, piece);

            await Repository.Update(piece);

            return WriteResult.FromValue(new DocumentMeta { Id = piece.Id });
        }

        public async Task<WriteResult> DeletePiece(Guid id)
        {
            await Repository.Delete(id);

            return WriteResult.Successful;
        }
    }
}