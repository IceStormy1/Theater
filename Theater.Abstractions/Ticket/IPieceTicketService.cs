using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Common;
using Theater.Contracts.Theater.PiecesTicket;

namespace Theater.Abstractions.Ticket;

public interface IPieceTicketService : ICrudService<PiecesTicketParameters>
{
    /// <summary>
    /// Получить билеты указанной пьесы по идентификатору даты пьесы
    /// </summary>
    /// <param name="pieceId">Идентификатор пьесы</param>
    /// <param name="dateId">Идентификатор даты пьесы</param>
    /// <returns></returns>
    Task<IReadOnlyCollection<PiecesTicketModel>> GetPieceTicketsByDate(Guid pieceId, Guid dateId);

    /// <summary>
    /// Купить билет 
    /// </summary>
    /// <param name="ticketId">Идентификатор билета</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Результат покупки</returns>
    Task<WriteResult> BuyTicket(Guid ticketId, Guid userId);

    /// <summary>
    /// Забронировать билет билет 
    /// </summary>
    /// <param name="ticketId">Идентификатор билета</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Результат бронирования</returns>
    Task<WriteResult> BookTicket(Guid ticketId, Guid userId);

    /// <summary>
    /// Добавить билеты для пьесы
    /// </summary>
    /// <param name="pieceId">Идентификатор пьесы</param>
    /// <param name="ticketsParameters">Билеты</param>
    Task<WriteResult> CreateTickets(Guid pieceId, PieceTicketCreateParameters ticketsParameters);

    /// <summary>
    /// Добавить билеты для пьесы
    /// </summary>
    /// <param name="pieceId">Идентификатор пьесы</param>
    /// <param name="ticketsParameters">Билеты</param>
    Task<WriteResult> UpdateTickets(Guid pieceId, PieceTicketUpdateParameters ticketsParameters);
}