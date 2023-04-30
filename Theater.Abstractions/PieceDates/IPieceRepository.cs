using System;
using System.Threading.Tasks;
using Theater.Entities.Theater;

namespace Theater.Abstractions.PieceDates;

public interface IPieceRepository : ICrudRepository<PieceEntity>
{
    /// <summary>
    /// Получить пьесу с датами
    /// </summary>
    /// <param name="pieceId">Идентификатор пьесы</param>
    Task<PieceEntity> GetPieceWithDates(Guid pieceId);
}