using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Ticket;
using Theater.Abstractions.UserAccount;
using Theater.Abstractions.UserAccount.Models;
using Theater.Common;
using Theater.Contracts.Theater;
using Theater.Entities.Authorization;
using Theater.Entities.Theater;

namespace Theater.Core.Ticket
{
    public class TicketService : ServiceBase<ITicketRepository>, ITicketService
    {
        private readonly IUserAccountRepository _userAccountRepository;

        public TicketService(
            IMapper mapper, 
            ITicketRepository repository, 
            IUserAccountRepository userAccountRepository) : base(mapper, repository)
        {
            _userAccountRepository = userAccountRepository;
        }

        public async Task<IReadOnlyCollection<PiecesTicketModel>> GetPieceTicketsByDate(Guid pieceId, Guid dateId)
        {
            var tickets = await Repository.GetPieceTicketsByDate(pieceId, dateId);

            return Mapper.Map<IReadOnlyCollection<PiecesTicketModel>>(tickets);
        }

        public async Task<WriteResult> BuyTicket(Guid ticketId, Guid userId)
        {
            var user = await _userAccountRepository.GetUserById(userId);

            if (user is null)
                return WriteResult.FromError(UserAccountErrors.NotFound.Error);

            if (user.Money == default)
                return WriteResult.FromError(UserAccountErrors.NotEnoughMoney.Error);

            var ticket = await Repository.GetPieceTicketById(ticketId);

            if (ticket is null)
                return WriteResult.FromError(TicketErrors.NotFound.Error);

            var validationResult = await CheckIfCanBuyTicket(ticket, user);

            return !validationResult.IsSuccess 
                ? validationResult 
                : await Repository.BuyTicket(ticket, user);
        }

        private async Task<WriteResult> CheckIfCanBuyTicket(PiecesTicketEntity ticket, UserEntity user)
        {
            if (ticket.BookedTickets.Any(x => x.UserId != user.Id))
                return WriteResult.FromError(TicketErrors.AlreadyBooked.Error);

            return ticket.TicketPrice > user.Money 
                ? WriteResult.FromError(UserAccountErrors.NotEnoughMoney.Error) 
                : WriteResult.Successful;
        }
    }
}