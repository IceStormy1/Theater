using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Ticket;
using Theater.Abstractions.UserAccount;
using Theater.Abstractions.UserAccount.Models;
using Theater.Common;
using Theater.Contracts.Theater;
using Theater.Entities.Authorization;
using Theater.Entities.Theater;

namespace Theater.Core.Ticket
{
    public sealed class TicketService : ServiceBase<PiecesTicketParameters, PiecesTicketEntity>, ITicketService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly ITicketRepository _ticketRepository;

        public TicketService(
            IMapper mapper, 
            ITicketRepository repository,
            IDocumentValidator<PiecesTicketParameters> documentValidator,
            IUserAccountRepository userAccountRepository) : base(mapper, repository, documentValidator)
        {
            _userAccountRepository = userAccountRepository;
            _ticketRepository = repository;
        }

        public async Task<IReadOnlyCollection<PiecesTicketModel>> GetPieceTicketsByDate(Guid pieceId, Guid dateId)
        {
            var tickets = await _ticketRepository.GetPieceTicketsByDate(pieceId, dateId);

            return Mapper.Map<IReadOnlyCollection<PiecesTicketModel>>(tickets);
        }

        public async Task<WriteResult> BuyTicket(Guid ticketId, Guid userId)
        {
            var user = await _userAccountRepository.GetByEntityId(userId);

            if (user is null)
                return WriteResult.FromError(UserAccountErrors.NotFound.Error);

            if (user.Money == default)
                return WriteResult.FromError(UserAccountErrors.NotEnoughMoney.Error);

            var ticket = await _ticketRepository.GetPieceTicketById(ticketId);

            if (ticket is null)
                return WriteResult.FromError(TicketErrors.NotFound.Error);

            var validationResult = CheckIfCanBuyTicket(ticket, user);

            return !validationResult.IsSuccess 
                ? validationResult 
                : await _ticketRepository.BuyTicket(ticket, user);
        }

        public async Task<WriteResult> BookTicket(Guid ticketId, Guid userId)
        {
            var user = await _userAccountRepository.GetByEntityId(userId);

            if (user is null)
                return WriteResult.FromError(UserAccountErrors.NotFound.Error);

            if (user.Money == default)
                return WriteResult.FromError(UserAccountErrors.NotEnoughMoney.Error);

            var ticket = await _ticketRepository.GetPieceTicketById(ticketId);

            if (ticket is null)
                return WriteResult.FromError(TicketErrors.NotFound.Error);

            var validationResult = CheckIfCanBookTicket(ticket, user);

            return !validationResult.IsSuccess
                ? validationResult
                : await _ticketRepository.BookTicket(ticketId, userId);
        }

        private static WriteResult CheckIfCanBuyTicket(PiecesTicketEntity ticket, UserEntity user)
        {
            if (ticket.BookedTickets.Any(x => x.UserId != user.Id))
                return WriteResult.FromError(TicketErrors.AlreadyBooked.Error);

            // TODO: валидация на уже купленный билет

            return ticket.TicketPrice > user.Money 
                ? WriteResult.FromError(UserAccountErrors.NotEnoughMoney.Error) 
                : WriteResult.Successful;
        }

        private static WriteResult CheckIfCanBookTicket(PiecesTicketEntity ticket, UserEntity user)
        {
            if (ticket.BookedTickets.Any())
                return WriteResult.FromError(TicketErrors.AlreadyBooked.Error);

            return ticket.TicketPrice > user.Money
                ? WriteResult.FromError(UserAccountErrors.NotEnoughMoney.Error)
                : WriteResult.Successful;
        }
    }
}