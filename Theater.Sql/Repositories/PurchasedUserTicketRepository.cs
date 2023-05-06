using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using Theater.Abstractions.PurchasedUserTicket;
using Theater.Entities.Theater;

namespace Theater.Sql.Repositories
{
    public sealed class PurchasedUserTicketRepository : BaseCrudRepository<PurchasedUserTicketEntity>, IPurchasedUserTicketRepository
    {
        public PurchasedUserTicketRepository(
            TheaterDbContext dbContext,
            ILogger<BaseCrudRepository<PurchasedUserTicketEntity>> logger) : base(dbContext, logger)
        {
        }

        public override IQueryable<PurchasedUserTicketEntity> AddIncludes(IQueryable<PurchasedUserTicketEntity> query)
        {
            return query.Include(x => x.TicketPriceEvents)
                .ThenInclude(x => x.PiecesTicket)
                .ThenInclude(x => x.PieceDate)
                .ThenInclude(x => x.Piece);
        }
    }
}
