using AutoMapper;
using Theater.Contracts.Theater.PurchasedUserTicket;
using Theater.Entities.Theater;

namespace Theater.Core.Profiles;

public sealed class PurchasedUserTicketProfile : Profile
{
    public PurchasedUserTicketProfile()
    {
        CreateMap<PurchasedUserTicketEntity, PurchasedUserTicketModel>()
            .ForMember(destination => destination.PieceDate, options => options.MapFrom(exp => exp.TicketPriceEvents.PiecesTicket.PieceDate.Date))
            .ForMember(destination => destination.PieceDateId, options => options.MapFrom(exp => exp.TicketPriceEvents.PiecesTicket.PieceDateId))
            .ForMember(destination => destination.PieceName, options => options.MapFrom(exp => exp.TicketPriceEvents.PiecesTicket.PieceDate.Piece.PieceName))
            .ForMember(destination => destination.PieceId, options => options.MapFrom(exp => exp.TicketPriceEvents.PiecesTicket.PieceDate.PieceId))
            .ForMember(destination => destination.TicketRow, options => options.MapFrom(exp => exp.TicketPriceEvents.Model.TicketRow))
            .ForMember(destination => destination.TicketPlace, options => options.MapFrom(exp => exp.TicketPriceEvents.Model.TicketPlace))
            .ForMember(destination => destination.TicketPrice, options => options.MapFrom(exp => exp.TicketPriceEvents.Model.TicketPrice))
            ;
    }
}