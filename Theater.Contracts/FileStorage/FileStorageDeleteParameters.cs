using System.Collections.Generic;

namespace Theater.Contracts.FileStorage;

public sealed class FileStorageDeleteParameters : FileStorageParameters
{
    /// <summary>
    /// Список названий файлов
    /// </summary>
    public IList<string> FileStorageName { get; set; }
}