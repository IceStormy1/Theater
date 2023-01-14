using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions.Piece.Models;

namespace Theater.Abstractions.Piece
{
    public interface IPieceRepository
    {
        /// <summary>
        /// Получить краткую информацию об актуальных пьесах
        /// </summary>
        Task<IReadOnlyCollection<PieceShortInformationDto>> GetPieceShortInformation();

        /// <summary>
        /// Получить полную информацию о пьесе по идентификатору
        /// </summary>
        /// <returns>Полная информация о пьесе</returns>
        Task<PieceDto> GetPieceById(Guid pieceId);
    }
}
