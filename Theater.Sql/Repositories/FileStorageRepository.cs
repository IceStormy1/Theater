using Microsoft.Extensions.Logging;
using Theater.Abstractions.FileStorage;
using Theater.Entities.FileStorage;

namespace Theater.Sql.Repositories;

public sealed class FileStorageRepository : BaseCrudRepository<FileStorageEntity>, IFileStorageRepository
{
    public FileStorageRepository(
        TheaterDbContext dbContext,
        ILogger<BaseCrudRepository<FileStorageEntity>> logger) : base(dbContext, logger)
    {
    }
}