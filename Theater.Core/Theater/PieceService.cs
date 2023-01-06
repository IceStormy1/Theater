using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions.Piece;
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
    }
}