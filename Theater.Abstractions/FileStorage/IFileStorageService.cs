using System;
using System.IO;
using System.Threading.Tasks;
using Theater.Common;
using Theater.Contracts.FileStorage;

namespace Theater.Abstractions.FileStorage;

public interface IFileStorageService
{
    /// <summary>
    /// Загрузить файл в хранилище S3 и записать информацию о файле в базу данных
    /// </summary>
    /// <param name="bucketIdentifier"></param>
    /// <param name="fileStream"></param>
    /// <param name="fileName"></param>
    /// <param name="userName">Имя пользователя</param>
    /// <returns></returns>
    Task<StorageFileListItem> Upload(BucketIdentifier bucketIdentifier, Stream fileStream,
        string fileName,
        string userName = null);

    /// <summary>
    /// Получить ссылку на загрузку файла по идентификатору файла
    /// </summary>
    /// <param name="entityId">Уникальный идентификатор файла</param>
    /// <returns></returns>
    Task<string> GetUrlById(Guid entityId);

    /// <summary>
    /// Получить полную информацию по идентификатору файла
    /// </summary>
    /// <param name="entityId">Уникальный идентификатор файла</param>
    /// <returns></returns>
    Task<StorageFileInfo> GetStorageFileInfo(Guid entityId);

    /// <summary>
    /// Получить поток на файл для скачивания
    /// </summary>
    /// <param name="entityId">Уникальный идентификатор файла</param>
    /// <param name="bucketName">Наименование бакета</param>
    /// <param name="fileStorageName">Наименование файла</param>
    /// <returns></returns>
    Task<Stream> GetFileStreamById(Guid entityId, string bucketName, string fileStorageName);

    /// <summary>
    /// Удалить файл
    /// </summary>
    /// <param name="entityId">Уникальный идентификатор файла</param>
    /// <param name="userName">Имя пользователя</param>
    /// <returns></returns>
    Task DeleteFileById(Guid entityId, string userName = null);
}