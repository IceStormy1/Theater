using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Ticket;
using Theater.Common;
using Theater.Entities.Authorization;
using Theater.Entities.Theater;

namespace Theater.Sql.Repositories
{
    public sealed class TicketRepository : ITicketRepository
    {
        private readonly TheaterDbContext _theaterDbContext;

        public TicketRepository(TheaterDbContext theaterDbContext)
        {
            _theaterDbContext = theaterDbContext;
        }

        public async Task<IReadOnlyCollection<PiecesTicketEntity>> GetPieceTicketsByDate(Guid pieceId, Guid dateId)
        {
            var piecesDates = await _theaterDbContext.PieceDates
                .AsNoTracking()
                .Where(x => x.Id == dateId && x.PieceId == pieceId)
                .Include(x => x.PiecesTickets)
                .SelectMany(x => x.PiecesTickets)
                .ToListAsync();

            return piecesDates;
        }

        public async Task<PiecesTicketEntity> GetPieceTicketById(Guid ticketId)
        {
            return await _theaterDbContext.PiecesTickets
                .AsNoTracking()
                .Include(x => x.BookedTickets)
                .Include(x => x.TicketPriceEvents)
                .ThenInclude(x => x.PurchasedUserTicket)
                .FirstOrDefaultAsync(x => x.Id == ticketId);
        }

        public async Task<WriteResult> BuyTicket(PiecesTicketEntity ticket, UserEntity user)
        {
            user.Money -= ticket.TicketPrice;
            var ticketPriceEvent = ticket.TicketPriceEvents.OrderByDescending(x => x.Timestamp).FirstOrDefault();

            if(ticketPriceEvent is null)
                return WriteResult.FromError(TicketErrors.NotFound.Error);

            var transaction = await _theaterDbContext.Database.BeginTransactionAsync();

            try
            {
                _theaterDbContext.Users.Update(user);
                _theaterDbContext.BookedTickets.RemoveRange(ticket.BookedTickets);

                var purchasedUserTicket = BuildPurchasedUserTicketEntity(ticketPriceEvent.PiecesTicketId, ticketPriceEvent.Version, user.Id);
                _theaterDbContext.PurchasedUserTickets.Add(purchasedUserTicket);

                await _theaterDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return WriteResult.Successful;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
               
                return WriteResult.FromError(TicketErrors.BuyTicketConflict.Error);
            }
        }

        public async Task<WriteResult> BookTicket(Guid ticketId, Guid userId)
        {
            try
            {
                _theaterDbContext.BookedTickets.Add(new BookedTicketEntity
                {
                    PiecesTicketId = ticketId,
                    Timestamp = DateTime.UtcNow,
                    UserId = userId
                });

                await _theaterDbContext.SaveChangesAsync();

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
}