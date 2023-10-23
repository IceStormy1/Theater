using AutoMapper;
using Theater.Abstractions.Piece.Models;
using Theater.Contracts.Theater.PieceDate;
using Theater.Entities.Theater;

namespace Theater.Core.Profiles;

public sealed class PieceDateProfile : Profile
{
    public PieceDateProfile()
    {
        CreateMap<PieceDateDto, PieceDateModel>();
        CreateMap<PieceDateParameters, PieceDateEntity>();

        CreateMap<PieceDateEntity, PieceDateModel>();
    }
}