using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Entities.Theater;

namespace Theater.Abstractions.Ticket
{
    public interface ITicketRepository
    {
        /// <summary>
        /// Получить билеты указанной пьесы по идентификатору даты пьесы
        /// </summary>
        /// <param name="pieceId">Идентификатор пьесы</param>
        /// <param name="dateId">Идентификатор даты пьесы</param>
        /// <returns></returns>
        Task<IReadOnlyCollection<PiecesTicketEntity>> GetPieceTicketsByDate(Guid pieceId, Guid dateId);
    }
}