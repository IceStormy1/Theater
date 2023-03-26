using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Common;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Abstractions.Piece
{
    public interface IPieceService : ICrudService<PieceParameters, PieceEntity>
    {
        /// <summary>
        /// Получить краткую информацию об актуальных пьесах
        /// </summary>
        Task<IReadOnlyCollection<PieceShortInformationModel>> GetPiecesShortInformation();

        /// <summary>
        /// Получить полную информацию о пьесе по идентификатору
        /// </summary>
        /// <returns>Полная информация о пьесе</returns>
        Task<WriteResult<PieceModel>> GetPieceById(Guid pieceId);
    }
}