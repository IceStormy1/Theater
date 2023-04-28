using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Abstractions.PieceDates;

public interface IPieceDateService : ICrudService<PieceDateParameters, PieceDateEntity>
{
}