using AutoMapper;
using Theater.Contracts.Theater.PieceWorker;
using Theater.Entities.Theater;

namespace Theater.Core.Profiles;

public sealed class PieceWorkerProfile : Profile
{
    public PieceWorkerProfile()
    {
        CreateMap<PieceWorkerParameters, PieceWorkerEntity>();
    }
}