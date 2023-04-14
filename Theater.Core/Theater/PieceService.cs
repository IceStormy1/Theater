using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Errors;
using Theater.Abstractions.Piece;
using Theater.Abstractions.PieceDates;
using Theater.Common;
using Theater.Contracts;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Core.Theater
{
    public sealed class PieceService : ServiceBase<PieceParameters, PieceEntity>, IPieceService
    {
        private readonly IPieceRepository _pieceRepository;
        private readonly IPieceDateRepository _pieceDateRepository;
        private readonly ICrudRepository<PieceWorkerEntity> _pieceWorkerCrudRepository;

        public PieceService(
            IMapper mapper,
            IPieceRepository repository, 
            IDocumentValidator<PieceParameters> documentValidator,
            IPieceDateRepository pieceDateRepository,
            ICrudRepository<PieceWorkerEntity> pieceWorkerCrudRepository) : base(mapper, repository, documentValidator)
        {
            _pieceRepository = repository;
            _pieceDateRepository = pieceDateRepository;
            _pieceWorkerCrudRepository = pieceWorkerCrudRepository;
        }

        public async Task<IReadOnlyCollection<PieceShortInformationModel>> GetPiecesShortInformation()
        {
            var piecesShortInformation = await _pieceRepository.GetPiecesShortInformation();

            return Mapper.Map<IReadOnlyCollection<PieceShortInformationModel>>(piecesShortInformation);
        }

        public async Task<WriteResult<PieceModel>> GetPieceById(Guid pieceId)
        {
            var pieceEntity = await _pieceRepository.GetByEntityId(pieceId);

            if(pieceEntity is null)
                return WriteResult<PieceModel>.FromError(PieceErrors.NotFound.Error);

            var pieceResult = Mapper.Map<PieceModel>(pieceEntity);

            return WriteResult.FromValue(pieceResult);
        }

        public async Task<WriteResult<DocumentMeta>> CreatePieceDate(PieceDateParameters parameters)
        {
            var pieceEntity = await _pieceRepository.GetPieceWithDates(parameters.PieceId);

            if(pieceEntity is null)
                return WriteResult<DocumentMeta>.FromError(PieceErrors.NotFound.Error);

            if(pieceEntity.PieceDates.Any(x=>x.Date.Date == parameters.Date))
                return WriteResult<DocumentMeta>.FromError(PieceErrors.DateAlreadyExists.Error);

            var pieceDateEntity = Mapper.Map<PieceDateEntity>(parameters);

            await _pieceDateRepository.Add(pieceDateEntity);

            return WriteResult.FromValue(new DocumentMeta(pieceDateEntity.Id));
        }
    }
}