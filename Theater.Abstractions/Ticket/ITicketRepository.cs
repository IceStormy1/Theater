using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Common;
using Theater.Entities.Authorization;
using Theater.Entities.Theater;

namespace Theater.Abstractions.Ticket;

public interface ITicketRepository : ICrudRepository<PiecesTicketEntity>
{
    /// <summary>
    /// Получить билеты указанной пьесы по идентификатору даты пьесы
    /// </summary>
    /// <param name="pieceId">Идентификатор пьесы</param>
    /// <param name="dateId">Идентификатор даты пьесы</param>
    /// <returns></returns>
    Task<IReadOnlyCollection<PiecesTicketEntity>> GetPieceTicketsByDate(Guid pieceId, Guid dateId);

    /// <summary>
    /// Получить билет по его идентификатору
    /// </summary>
    /// <param name="ticketId">Идентификатор билета</param>
    Task<PiecesTicketEntity> GetPieceTicketById(Guid ticketId);

    /// <summary>
    /// Купить билет
    /// </summary>
    /// <param name="ticket">Сущность билета</param>
    /// <param name="user">Сущность пользователя</param>
    Task<WriteResult> BuyTicket(PiecesTicketEntity ticket, UserEntity user);

    /// <summary>
    /// Забронировать билет
    /// </summary>
    /// <param name="ticketId">Идентификатор билета</param>
    /// <param name="userId">Идентификатор пользователя</param>
    Task<WriteResult> BookTicket(Guid ticketId, Guid userId);
}