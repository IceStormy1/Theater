using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Errors;
using Theater.Abstractions.PieceDates;
using Theater.Abstractions.Ticket;
using Theater.Abstractions.UserAccount;
using Theater.Common;
using Theater.Contracts.Theater;
using Theater.Entities.Authorization;
using Theater.Entities.Theater;

namespace Theater.Core.Ticket;

public sealed class PieceTicketService : ServiceBase<PiecesTicketParameters, PiecesTicketEntity>, IPieceTicketService
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IPieceRepository _pieceRepository;
    private readonly ILogger<PieceTicketService> _logger;

    public PieceTicketService(
        IMapper mapper, 
        ITicketRepository repository,
        IDocumentValidator<PiecesTicketParameters> documentValidator,
        IUserAccountRepository userAccountRepository,
        IPieceRepository pieceRepository, 
        ILogger<PieceTicketService> logger) : base(mapper, repository, documentValidator)
    {
        _userAccountRepository = userAccountRepository;
        _pieceRepository = pieceRepository;
        _logger = logger;
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

    public async Task<WriteResult> CreateTickets(Guid pieceId, PieceTicketCreateParameters ticketsParameters)
    {
        var pieceEntity = await _pieceRepository.GetByEntityId(pieceId);
        // todo: валидация, что дата билета больше или равна текущей даты
        if (pieceEntity is null)
            return WriteResult.FromError(PieceErrors.NotFound.Error);

        var piecesTicketEntities = await _ticketRepository.GetPieceTicketsByDate(pieceId, ticketsParameters.PieceDateId);

        if(piecesTicketEntities.Count == default)
            return WriteResult.FromError(TicketErrors.TicketsAlreadyCreated.Error);

        var ticketEntities = Mapper.Map<List<PiecesTicketEntity>>(ticketsParameters.PiecesTickets);
        ticketEntities.ForEach(x => x.PieceDateId = ticketsParameters.PieceDateId);

        try
        {
            await _ticketRepository.AddRange(ticketEntities);

            return WriteResult.Successful;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Произошла ошибка при попытке добавить билеты в БД");

            return WriteResult.FromError(TicketErrors.CreateTicketConflict.Error);
        }
    }

    public async Task<WriteResult> UpdateTickets(Guid pieceId, PieceTicketUpdateParameters ticketsParameters)
    {
        var pieceEntity = await _pieceRepository.GetByEntityId(pieceId);

        if (pieceEntity is null)
            return WriteResult.FromError(PieceErrors.NotFound.Error);

        var ticketsIds = ticketsParameters.PiecesTickets.Select(x => x.Id).ToList();
        var ticketsEntities = await _ticketRepository.GetByEntityIds(ticketsIds);

        if(ticketsEntities.Count != ticketsParameters.PiecesTickets.Count)
            return WriteResult.FromError(TicketErrors.NotFound.Error);

        foreach (var piecesTicketEntity in ticketsEntities)
        {
            var ticketModel = ticketsParameters.PiecesTickets.First(x => x.Id == piecesTicketEntity.Id);
            Mapper.Map(ticketModel, piecesTicketEntity);
        }

        try
        {
            await _ticketRepository.UpdateRange(ticketsEntities);

            return WriteResult.Successful;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Произошла ошибка при попытке добавить билеты в БД");

            return WriteResult.FromError(TicketErrors.CreateTicketConflict.Error);
        }
    }

    private static WriteResult CheckIfCanBuyTicket(PiecesTicketEntity ticket, UserEntity user)
    {
        if (ticket.BookedTicket.UserId != user.Id)
            return WriteResult.FromError(TicketErrors.AlreadyBooked.Error);

        // TODO: валидация на уже купленный билет

        return ticket.TicketPrice > user.Money 
            ? WriteResult.FromError(UserAccountErrors.NotEnoughMoney.Error) 
            : WriteResult.Successful;
    }

    private static WriteResult CheckIfCanBookTicket(PiecesTicketEntity ticket, UserEntity user)
    {
        if (ticket.BookedTicket != null)
            return WriteResult.FromError(TicketErrors.AlreadyBooked.Error);

        return ticket.TicketPrice > user.Money
            ? WriteResult.FromError(UserAccountErrors.NotEnoughMoney.Error)
            : WriteResult.Successful;
    }
}