namespace Theater.Contracts.FileStorage;

/// <summary>
/// Модель записи с информацией о файле в хранилище S3
/// </summary>
public class StorageFileInfo : StorageFileListItem
{
    /// <summary>
    /// Bucket
    /// </summary>
    public string Bucket { get; set; }

    /// <summary>
    /// Bucket
    /// </summary>
    public string StorageFileName { get; set; }

    /// <summary>
    /// ContentType
    /// </summary>
    public string ContentType { get; set; }
}