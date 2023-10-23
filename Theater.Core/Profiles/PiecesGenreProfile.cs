using AutoMapper;
using Theater.Contracts.Theater.PiecesGenre;
using Theater.Entities.Theater;

namespace Theater.Core.Profiles;

public sealed class PiecesGenreProfile : Profile
{
    public PiecesGenreProfile()
    {
        CreateMap<PiecesGenreEntity, PiecesGenreModel>();
        CreateMap<PiecesGenreParameters, PiecesGenreEntity>();
    }
}