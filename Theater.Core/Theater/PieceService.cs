using System;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions.Piece;
using Theater.Abstractions.Piece.Models;
using Theater.Common;
using Theater.Contracts.Theater;

namespace Theater.Core.Theater
{
    public class PieceService : IPieceService
    {
        private readonly IPieceRepository _pieceRepository;
        private readonly IMapper _mapper;

        public PieceService(IPieceRepository pieceRepository, IMapper mapper)
        {
            _pieceRepository = pieceRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<PieceShortInformationModel>> GetPieceShortInformation()
        {
            var piecesShortInformation = await _pieceRepository.GetPieceShortInformation();

            return _mapper.Map<IReadOnlyCollection<PieceShortInformationModel>>(piecesShortInformation);
        }

        public async Task<WriteResult<PieceModel>> GetPieceById(Guid pieceId)
        {
            var pieceDto = await _pieceRepository.GetPieceById(pieceId);

            if(pieceDto is null)
                return WriteResult<PieceModel>.FromError(PieceErrors.NotFound.Error);

            var pieceResult = _mapper.Map<PieceModel>(pieceDto);

            return WriteResult.FromValue(pieceResult);
        }
    }
}