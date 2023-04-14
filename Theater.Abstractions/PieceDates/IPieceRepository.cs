using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions.Piece.Models;
using Theater.Entities.Theater;

namespace Theater.Abstractions.PieceDates
{
    public interface IPieceRepository : ICrudRepository<PieceEntity>
    {
        /// <summary>
        /// Получить краткую информацию об актуальных пьесах
        /// </summary>
        Task<IReadOnlyCollection<PieceShortInformationDto>> GetPiecesShortInformation();

        /// <summary>
        /// Получить пьесу с датами
        /// </summary>
        /// <param name="pieceId">Идентификатор пьесы</param>
        Task<PieceEntity> GetPieceWithDates(Guid pieceId);
    }
}
