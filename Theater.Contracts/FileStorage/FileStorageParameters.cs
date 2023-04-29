namespace Theater.Contracts.FileStorage;

public abstract class FileStorageParameters
{
    /// <summary>
    /// Название бакета
    /// </summary>
    public string BucketName { get; set; }
}