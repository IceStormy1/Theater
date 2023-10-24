using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Common;
using Theater.Entities.Theater;
using Theater.Entities.Users;

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
    /// Купить билеты
    /// </summary>
    /// <param name="tickets">Сущности билетов</param>
    /// <param name="user">Сущность пользователя</param>
    Task<Result> BuyTickets(IReadOnlyCollection<PiecesTicketEntity> tickets, UserEntity user);

    /// <summary>
    /// Забронировать билет
    /// </summary>
    /// <param name="ticketIds">Идентификаторы билетов</param>
    /// <param name="userId">Идентификатор пользователя</param>
    Task<Result> BookTicket(IReadOnlyCollection<Guid> ticketIds, Guid userId);
}