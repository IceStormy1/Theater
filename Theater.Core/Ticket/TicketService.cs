using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions.Ticket;
using Theater.Contracts.Theater;

namespace Theater.Core.Ticket
{
    public class TicketService : ServiceBase<ITicketRepository>, ITicketService
    {
        public TicketService(IMapper mapper, ITicketRepository repository) : base(mapper, repository)
        {
        }

        public async Task<IReadOnlyCollection<PiecesTicketModel>> GetPieceTicketsByDate(Guid pieceId, Guid dateId)
        {
            var tickets = await Repository.GetPieceTicketsByDate(pieceId, dateId);

            return Mapper.Map<IReadOnlyCollection<PiecesTicketModel>>(tickets);
        }
    }
}