using AutoMapper;
using Theater.Contracts.FileStorage;
using Theater.Entities.FileStorage;

namespace Theater.Core.Profiles;

public sealed class FileStorageProfile : Profile
{
    public FileStorageProfile()
    {
        CreateMap<FileStorageEntity, StorageFileListItem>();
    }
}