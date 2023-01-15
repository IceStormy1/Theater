using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Ticket;
using Theater.Entities.Theater;

namespace Theater.Sql.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TheaterDbContext _theaterDbContext;

        public TicketRepository(TheaterDbContext theaterDbContext)
        {
            _theaterDbContext = theaterDbContext;
        }

        public async Task<IReadOnlyCollection<PiecesTicketEntity>> GetPieceTicketsByDate(Guid pieceId, Guid dateId)
        {
            var ff = await _theaterDbContext.PieceDates
                .AsNoTracking()
                .Where(x => x.Id == dateId && x.PieceId == pieceId)
                .Include(x => x.PiecesTickets)
                .SelectMany(x => x.PiecesTickets)
                .ToListAsync();

            return ff;
        }
    }
}