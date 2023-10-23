using AutoMapper;
using Theater.Abstractions;
using Theater.Contracts;

namespace Theater.Core.Profiles;

public sealed class AbstractProfile : Profile
{
    public AbstractProfile()
    {
        CreateMap(typeof(PagingResult<>), typeof(Page<>));
    }
}