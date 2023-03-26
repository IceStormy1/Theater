using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions.Piece;
using Theater.Abstractions.Piece.Models;
using Theater.Common;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Core.Theater
{
    public sealed class PieceService : ServiceBase<PieceParameters, PieceEntity>, IPieceService
    {
        private readonly IPieceRepository _pieceRepository;

        public PieceService(
            IMapper mapper,
            IPieceRepository repository) : base(mapper, repository)
        {
            _pieceRepository = repository;
        }

        public async Task<IReadOnlyCollection<PieceShortInformationModel>> GetPiecesShortInformation()
        {
            var piecesShortInformation = await _pieceRepository.GetPiecesShortInformation();

            return Mapper.Map<IReadOnlyCollection<PieceShortInformationModel>>(piecesShortInformation);
        }

        public async Task<WriteResult<PieceModel>> GetPieceById(Guid pieceId)
        {
            var pieceDto = await _pieceRepository.GetPieceDtoById(pieceId);

            if(pieceDto is null)
                return WriteResult<PieceModel>.FromError(PieceErrors.NotFound.Error);

            var pieceResult = Mapper.Map<PieceModel>(pieceDto);

            return WriteResult.FromValue(pieceResult);
        }


    }
}