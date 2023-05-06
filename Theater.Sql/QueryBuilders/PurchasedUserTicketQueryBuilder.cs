using LinqKit;
using System;
using System.Linq.Expressions;
using Theater.Abstractions.Filters;
using Theater.Entities.Theater;
using Theater.Sql.Extensions;

namespace Theater.Sql.QueryBuilders
{
    public sealed class PurchasedUserTicketQueryBuilder : QueryBuilderBase<PurchasedUserTicketEntity, PieceTicketFilterSettings>
    {
        public override Expression<Func<PurchasedUserTicketEntity, bool>> BuildQueryFilter(PieceTicketFilterSettings filter)
        {
            var pb = PredicateBuilder.New<PurchasedUserTicketEntity>(x => true);

            pb.And(filter.UserId, x => x.UserId == filter.UserId);

            return pb;
        }

        protected override Expression<Func<PurchasedUserTicketEntity, object>> ResolveSortingExpression(string sortColumn)
        {
            return sortColumn?.ToLowerInvariant() switch
            {
                "piecename" => x => x.TicketPriceEvents.PiecesTicket.PieceDate.Piece.PieceName,
                "piecedate" => x => x.TicketPriceEvents.PiecesTicket.PieceDate.Date,
                "dateofpurchase" => x => x.DateOfPurchase,
                _ => x => x.DateOfPurchase
            };
        }
    }
}
