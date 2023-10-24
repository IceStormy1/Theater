using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Errors;
using Theater.Abstractions.Ticket;
using Theater.Common;
using Theater.Entities.Theater;
using Theater.Entities.Users;

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
            .OrderBy(x=>x.TicketRow).ThenBy(x=>x.TicketPlace)
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

    public async Task<Result> BuyTickets(IReadOnlyCollection<PiecesTicketEntity> tickets, UserEntity user)
    {
        var ticketsPriceEvents = tickets
            .Select(x => new { MaxVersion = x.TicketPriceEvents.MaxBy(c => c.Version) })
            .Where(x=>x.MaxVersion != null)
            .Select(x=>x.MaxVersion)
            .ToList();

        if (ticketsPriceEvents.Count != tickets.Count)
            return Result.FromError(TicketErrors.NotFound.Error);

        user.Money -= ticketsPriceEvents.Sum(x=>x.Model.TicketPrice);

        var transaction = await DbContext.Database.BeginTransactionAsync();

        try
        {
            var bookedTickets = tickets.Select(x => x.BookedTicket).Where(x => x != null);
            _dbContext.BookedTickets.RemoveRange(bookedTickets);

            _dbContext.Users.Update(user);

            var purchasedUserTicket = BuildPurchasedUserTicketEntity(ticketsPriceEvents, user.Id);
            _dbContext.PurchasedUserTickets.AddRange(purchasedUserTicket);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result.Successful;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            Logger.LogError(e, "Произошла ошибка при покупке билетов. UserId = {UserId}, TicketIds = {@TicketId}", user.Id, tickets.Select(x=>x.Id));
               
            return Result.FromError(TicketErrors.BuyTicketConflict.Error);
        }
    }

    public async Task<Result> BookTicket(IReadOnlyCollection<Guid> ticketIds, Guid userId)
    {
        try
        {
            _dbContext.BookedTickets.AddRange(ticketIds.Select(x=> new BookedTicketEntity
            {
                PiecesTicketId = x,
                Timestamp = DateTime.UtcNow,
                UserId = userId
            }));

            await _dbContext.SaveChangesAsync();

            return Result.Successful;
        }
        catch (Exception)
        {
            return Result.FromError(TicketErrors.BookTicketConflict.Error);
        }
    }

    private static List<PurchasedUserTicketEntity> BuildPurchasedUserTicketEntity(List<TicketPriceEventsEntity> ticketPriceEvents, Guid userId)
        => ticketPriceEvents.Select(x=> new PurchasedUserTicketEntity
        {
            DateOfPurchase = DateTime.UtcNow,
            TicketPriceEventsId = x.PiecesTicketId,
            TicketPriceEventsVersion = x.Version,
            UserId = userId
        }).ToList();
}