using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Errors;
using Theater.Abstractions.Ticket;
using Theater.Common;
using Theater.Entities.Authorization;
using Theater.Entities.Theater;

namespace Theater.Sql.Repositories;

public sealed class TicketRepository : BaseCrudRepository<PiecesTicketEntity>, ITicketRepository
{
    private readonly TheaterDbContext _dbContext;

    public TicketRepository(
        TheaterDbContext dbContext,
        ILogger<BaseCrudRepository<PiecesTicketEntity>> logger) : base(dbContext, logger)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<PiecesTicketEntity>> GetPieceTicketsByDate(Guid pieceId, Guid dateId)
    {
        var piecesDates = await _dbContext.PiecesTickets.AsNoTracking()
            .Where(x => x.PieceDateId == dateId && x.PieceDate.PieceId == pieceId)
            .Include(x => x.BookedTicket)
            .Include(x => x.TicketPriceEvents).ThenInclude(x => x.PurchasedUserTicket)
            .ToListAsync();

        return piecesDates;
    }

    public override IQueryable<PiecesTicketEntity> AddIncludes(IQueryable<PiecesTicketEntity> query)
    {
        return query
            .Include(x => x.BookedTicket)
            .Include(x => x.TicketPriceEvents)
            .ThenInclude(x => x.PurchasedUserTicket);
    }

    public async Task<WriteResult> BuyTicket(PiecesTicketEntity ticket, UserEntity user)
    {
        var ticketPriceEvent = ticket.TicketPriceEvents.MaxBy(x => x.Version);

        if (ticketPriceEvent is null)
            return WriteResult.FromError(TicketErrors.NotFound.Error);

        user.Money -= ticketPriceEvent.Model.TicketPrice;

        var transaction = await DbContext.Database.BeginTransactionAsync();

        try
        {
            _dbContext.Users.Update(user);

            if(ticket.BookedTicket != null)
                _dbContext.BookedTickets.RemoveRange(ticket.BookedTicket);

            var purchasedUserTicket = BuildPurchasedUserTicketEntity(ticketPriceEvent.PiecesTicketId, ticketPriceEvent.Version, user.Id);
            _dbContext.PurchasedUserTickets.Add(purchasedUserTicket);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return WriteResult.Successful;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            Logger.LogError(e, "Произошла ошибка при покупке билета. UserId = {UserId}, TicketId = {TicketId}", user.Id, ticket.Id);
               
            return WriteResult.FromError(TicketErrors.BuyTicketConflict.Error);
        }
    }

    public async Task<WriteResult> BookTicket(Guid ticketId, Guid userId)
    {
        try
        {
            _dbContext.BookedTickets.Add(new BookedTicketEntity
            {
                PiecesTicketId = ticketId,
                Timestamp = DateTime.UtcNow,
                UserId = userId
            });

            await _dbContext.SaveChangesAsync();

            return WriteResult.Successful;
        }
        catch (Exception)
        {
            return WriteResult.FromError(TicketErrors.BookTicketConflict.Error);
        }
    }

    private static PurchasedUserTicketEntity BuildPurchasedUserTicketEntity(Guid ticketPriceEventsId, int ticketPriceEventsVersion, Guid userId)
        => new()
        {
            DateOfPurchase = DateTime.UtcNow,
            TicketPriceEventsId = ticketPriceEventsId,
            TicketPriceEventsVersion = ticketPriceEventsVersion,
            UserId = userId
        };
}