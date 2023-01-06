using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions.Piece.Models;
using Theater.Contracts.Theater;

namespace Theater.Abstractions.Piece
{
    public interface IPieceService
    {
        /// <summary>
        /// Получить краткую информацию об актуальных пьесах
        /// </summary>
        Task<IReadOnlyCollection<PieceShortInformationModel>> GetPieceShortInformation();
    }
}
