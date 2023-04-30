using System;
using System.Threading.Tasks;
using Theater.Common;
using Theater.Contracts;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Abstractions.Piece;

public interface IPieceService : ICrudService<PieceParameters, PieceEntity>
{
    /// <summary>
    /// Получить полную информацию о пьесе по идентификатору
    /// </summary>
    /// <returns>Полная информация о пьесе</returns>
    Task<WriteResult<PieceModel>> GetPieceById(Guid pieceId);

    /// <summary>
    /// Обогатить модель
    /// </summary>
    Task EnrichPieceShortInformation(Page<PieceShortInformationModel> shortInformationModel);
}