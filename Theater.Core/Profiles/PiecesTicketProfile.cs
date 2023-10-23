using AutoMapper;
using System.Linq;
using Theater.Abstractions.Filters;
using Theater.Contracts.Filters;
using Theater.Contracts.Theater.PiecesTicket;
using Theater.Entities.Theater;

namespace Theater.Core.Profiles;

public sealed class PiecesTicketProfile : Profile
{
    public PiecesTicketProfile()
    {
        CreateMap<PiecesTicketEntity, PiecesTicketModel>()
            .ForMember(destination => destination.IsBooked, options => options.MapFrom(exp => exp.BookedTicket != null || exp.TicketPriceEvents.Any(c => c.PurchasedUserTicket != null)))
            ;

        CreateMap<PiecesTicketParameters, PiecesTicketEntity>();
        CreateMap<PiecesTicketModel, PiecesTicketEntity>();
        CreateMap<PieceTicketFilterParameters, PieceTicketFilterSettings>();
    }
}