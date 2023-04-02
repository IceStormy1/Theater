using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Piece;
using Theater.Abstractions.Piece.Models;
using Theater.Common;
using Theater.Contracts;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;
using Theater.Sql.Repositories;

namespace Theater.Core.Theater
{
    public sealed class PieceService : ServiceBase<PieceParameters, PieceEntity>, IPieceService
    {
        private readonly IPieceRepository _pieceRepository;
        private readonly IPieceDateRepository _pieceDateRepository;

        public PieceService(
            IMapper mapper,
            IPieceRepository repository, 
            IPieceDateRepository pieceDateRepository) : base(mapper, repository)
        {
            _pieceRepository = repository;
            _pieceDateRepository = pieceDateRepository;
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

        public async Task<WriteResult<DocumentMeta>> CreatePieceDate(Guid pieceId, DateTime date)
        {
            var pieceEntity = await _pieceRepository.GetPieceWithDates(pieceId);

            if(pieceEntity is null)
                return WriteResult<DocumentMeta>.FromError(PieceErrors.NotFound.Error);

            if(pieceEntity.PieceDates.Any(x=>x.Date.Date == date.Date))
                return WriteResult<DocumentMeta>.FromError(PieceErrors.DateAlreadyExists.Error);

            var pieceDateEntity = new PieceDateEntity { Date = date, PieceId = pieceId };

            await _pieceDateRepository.Add(pieceDateEntity);

            return WriteResult.FromValue(new DocumentMeta(pieceDateEntity.Id));
        }
    }
}