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
using Theater.Contracts.Theater.PiecesTicket;
using Theater.Entities.Authorization;
using Theater.Entities.Theater;

namespace Theater.Core.Theater.Services;

public sealed class PieceTicketService : ServiceBase<PiecesTicketParameters, PiecesTicketEntity>, IPieceTicketService
{
    private const int DefaultTicketVersion = 1;

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

    public async Task<WriteResult> BuyTickets(IReadOnlyCollection<Guid> ticketIds, Guid userId)
    {
        var user = await _userAccountRepository.GetByEntityId(userId, useAsNoTracking: true);

        if (user is null)
            return WriteResult.FromError(UserAccountErrors.NotFound.Error);

        if (user.Money == default)
            return WriteResult.FromError(UserAccountErrors.NotEnoughMoney.Error);

        var tickets = await _ticketRepository.GetByEntityIds(ticketIds);

        if (tickets.Count != ticketIds.Count)
            return WriteResult.FromError(TicketErrors.TicketsNotFound.Error);

        var validationResult = CheckIfCanBuyOrBookTicket(tickets, user);

        return !validationResult.IsSuccess
            ? validationResult
            : await _ticketRepository.BuyTickets(tickets, user);
    }

    public async Task<WriteResult> BookTicket(IReadOnlyCollection<Guid> ticketIds, Guid userId)
    {
        var user = await _userAccountRepository.GetByEntityId(userId);

        if (user is null)
            return WriteResult.FromError(UserAccountErrors.NotFound.Error);

        if (user.Money == default)
            return WriteResult.FromError(UserAccountErrors.NotEnoughMoney.Error);

        var tickets = await _ticketRepository.GetByEntityIds(ticketIds);

        if (tickets is null)
            return WriteResult.FromError(TicketErrors.NotFound.Error);

        var validationResult = CheckIfCanBookTicket(tickets, user);

        return !validationResult.IsSuccess
            ? validationResult
            : await _ticketRepository.BookTicket(ticketIds, userId);
    }

    public async Task<WriteResult> CreateTickets(Guid pieceId, PieceTicketCreateParameters ticketsParameters)
    {
        var pieceEntity = await _pieceRepository.GetByEntityId(pieceId);
        // todo: валидация, что дата билета больше или равна текущей даты
        if (pieceEntity is null)
            return WriteResult.FromError(PieceErrors.NotFound.Error);

        if (!pieceEntity.PieceDates.Any(x => x.Id == ticketsParameters.PieceDateId))
            return WriteResult.FromError(TicketErrors.WrongPieceDate.Error);

        var piecesTicketEntities = await _ticketRepository.GetPieceTicketsByDate(pieceId, ticketsParameters.PieceDateId);

        if (piecesTicketEntities.Count != default)
            return WriteResult.FromError(TicketErrors.TicketsAlreadyCreated.Error);

        var ticketEntities = Mapper.Map<List<PiecesTicketEntity>>(ticketsParameters.PiecesTickets);
        ticketEntities.ForEach(x =>
        {
            x.PieceDateId = ticketsParameters.PieceDateId;
            x.TicketPriceEvents = new List<TicketPriceEventsEntity>
            {
                new()
                {
                    Model = x,
                    PiecesTicket = x,
                    Timestamp = DateTime.UtcNow,
                    Version = DefaultTicketVersion
                }
            };
        });

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

        if (ticketsEntities.Count != ticketsParameters.PiecesTickets.Count)
            return WriteResult.FromError(TicketErrors.NotFound.Error);

        foreach (var piecesTicketEntity in ticketsEntities)
        {
            var ticketModel = ticketsParameters.PiecesTickets.First(x => x.Id == piecesTicketEntity.Id);
            Mapper.Map(ticketModel, piecesTicketEntity);
            piecesTicketEntity.TicketPriceEvents ??= new List<TicketPriceEventsEntity>();

            var lastTicketPrice = piecesTicketEntity.TicketPriceEvents.MaxBy(x => x.Version);

            piecesTicketEntity.TicketPriceEvents.Add(new TicketPriceEventsEntity
            {
                Model = piecesTicketEntity,
                PiecesTicket = piecesTicketEntity,
                Timestamp = DateTime.UtcNow,
                Version = lastTicketPrice is null ? DefaultTicketVersion : lastTicketPrice.Version + 1,
            });
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

    private static WriteResult CheckIfCanBuyOrBookTicket(IReadOnlyCollection<PiecesTicketEntity> tickets, UserEntity user, bool isBookEvent = false)
    {
        var alreadyBookedTicket = tickets.FirstOrDefault(x => x.BookedTicket != null && (isBookEvent || x.BookedTicket.UserId != user.Id));

        if(alreadyBookedTicket != null)
            return WriteResult.FromError(ErrorModel.Default(TicketErrors.AlreadyBooked.Error.Type, string.Format(TicketErrors.AlreadyBooked.Error.Message, alreadyBookedTicket.TicketRow, alreadyBookedTicket.TicketPlace)));

        var alreadyPurchasedTicket = tickets.FirstOrDefault(x => x.TicketPriceEvents.Any(c => c.PurchasedUserTicket != null));

        if(alreadyPurchasedTicket != null)
            return WriteResult.FromError(ErrorModel.Default(TicketErrors.AlreadyBought.Error.Type, string.Format(TicketErrors.AlreadyBought.Error.Message, alreadyPurchasedTicket.TicketRow, alreadyPurchasedTicket.TicketPlace)));

        return tickets.Sum(x=>x.TicketPrice) > user.Money
            ? WriteResult.FromError(UserAccountErrors.NotEnoughMoney.Error)
            : WriteResult.Successful;
    }

    private static WriteResult CheckIfCanBookTicket(IReadOnlyCollection<PiecesTicketEntity> tickets, UserEntity user)
    {
        return CheckIfCanBuyOrBookTicket(tickets, user, isBookEvent: true);
    }
}