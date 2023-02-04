using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions.Piece;
using Theater.Abstractions.Piece.Models;
using Theater.Common;
using Theater.Contracts.Theater;

namespace Theater.Core.Theater
{
    public sealed class PieceService : ServiceBase<IPieceRepository>, IPieceService
    {
        public PieceService(IMapper mapper, IPieceRepository repository) : base(mapper, repository)
        {
        }

        public async Task<IReadOnlyCollection<PieceShortInformationModel>> GetPiecesShortInformation()
        {
            var piecesShortInformation = await Repository.GetPiecesShortInformation();

            return Mapper.Map<IReadOnlyCollection<PieceShortInformationModel>>(piecesShortInformation);
        }

        public async Task<WriteResult<PieceModel>> GetPieceById(Guid pieceId)
        {
            var pieceDto = await Repository.GetPieceById(pieceId);

            if(pieceDto is null)
                return WriteResult<PieceModel>.FromError(PieceErrors.NotFound.Error);

            var pieceResult = Mapper.Map<PieceModel>(pieceDto);

            return WriteResult.FromValue(pieceResult);
        }
    }
}