using Amazon.S3;
using Amazon.S3.Model;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Theater.Abstractions.FileStorage;
using Theater.Common.Enums;
using Theater.Common.Extensions;
using Theater.Common.Settings;
using Theater.Contracts.FileStorage;
using Theater.Core.Utils;
using Theater.Entities.FileStorage;

namespace Theater.Core.FileStorage;

public sealed class FileStorageService : IFileStorageService
{
    private readonly IFileStorageRepository _fileStorageRepository;
    private readonly FileStorageOptions _fileStorageOptions;
    private readonly IAmazonS3 _s3Client;
    private readonly IMapper _mapper;
    private readonly ILogger<FileStorageService> _logger;

    public FileStorageService(
        IAmazonS3 s3Client,
        IMapper mapper,
        ILogger<FileStorageService> logger,
        IOptions<FileStorageOptions> fileStorageOptions,
        IFileStorageRepository fileStorageRepository)
    {
        _s3Client = s3Client;
        _logger = logger;
        _fileStorageRepository = fileStorageRepository;
        _mapper = mapper;
        _fileStorageOptions = fileStorageOptions?.Value;
    }

    public async Task<StorageFileListItem> Upload(
        BucketIdentifier bucketIdentifier,
        Stream fileStream,
        string fileNameWithExtension,
        string userName = null)
    {
        var bucketName = bucketIdentifier.ToBucketName();

        ValidateFileNameAndThrow(fileNameWithExtension);

        //generate data
        var (newFileGuid, fileStorageName) = GenerateUniqueFileName(fileNameWithExtension);

        var contentType = MimeTypeMap.GetMimeType(fileNameWithExtension);

        await WriteFile(
            bucketName,
            fileStorageName,
            fileStream,
            contentType);

        try
        {
            var fileEntity = new FileStorageEntity
            {
                Id = newFileGuid,
                BucketId = bucketIdentifier,
                ContentType = contentType,
                Size = (ulong)fileStream.Length,
                FileName = fileNameWithExtension,
                FileStorageName = fileStorageName
            };

            await _fileStorageRepository.Add(fileEntity);

            var result = _mapper.Map<StorageFileListItem>(fileEntity);
            result.DirectUrl = GetFileUrl(fileEntity);

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка во время сохранения сведений о загруженном файле в базу данных");

            throw;
        }
    }

    /// <summary>
    /// Валидирует имя файла и выбрасывает ошибку в случае, если валидация не была пройдена
    /// </summary>
    /// <param name="fileNameWithExtension">Имя файла с расширением</param>
    /// <returns></returns>
    private void ValidateFileNameAndThrow(string fileNameWithExtension)
    {
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithExtension);

        if (fileNameWithoutExtension.Length <= _fileStorageOptions.MaximumNameLength) 
            return;

        var fileNameLengthError = $"Имя файла превышает {_fileStorageOptions.MaximumNameLength} символов";
        _logger.LogWarning("Имя файла превышает {MaximumNameLength} символов", _fileStorageOptions.MaximumNameLength);

        throw new Exception(fileNameLengthError);
    }

    /// <inheritdoc cref="IFileStorageService.GetUrlById"/>
    public async Task<string> GetUrlById(Guid entityId)
    {
        var fileEntity = await GetFileEntity(entityId);

        return GetFileUrl(fileEntity);
    }

    private string GetFileUrl(FileStorageEntity fileEntity)
        => $"{_fileStorageOptions?.ServiceUrl}/{fileEntity.BucketId.ToBucketName()}/{fileEntity.FileStorageName}";

    public async Task<StorageFileInfo> GetStorageFileInfoById(Guid entityId)
    {
        var fileEntity = await GetFileEntity(entityId);

        var result = new StorageFileInfo
        {
            DirectUrl = $"{_fileStorageOptions?.ServiceUrl}/{fileEntity.BucketId.ToBucketName()}/{fileEntity.FileStorageName}",
            Bucket = fileEntity.BucketId.ToBucketName(),
            ContentType = fileEntity.ContentType,
            Id = entityId,
            FileName = fileEntity.FileName,
            Size = (long?)fileEntity.Size,
            StorageFileName = fileEntity.FileStorageName,
            UploadAt = fileEntity.CreatedAt
        };
        return result;
    }
    public async Task<Stream> GetFileStreamById(Guid entityId, string bucketName, string fileStorageName)
    {
        var stream = await GetFile(bucketName, fileStorageName);
        stream.Position = 0;
        return stream;
    }

    public async Task DeleteFileById(Guid entityId, string userName = null)
    {
        var fileEntity = await GetFileEntity(entityId);

        await RemoveFile(fileEntity.BucketId.ToBucketName(), fileEntity.FileStorageName);
    }

    private static string GetPseudoFolderSuffix()
    {
        var now = DateTime.UtcNow;
        return $"{now.Year}/{now.Month.ToString().PadLeft(2, '0')}/{now.Day.ToString().PadLeft(2, '0')}/";
    }

    private async Task<FileStorageEntity> GetFileEntity(Guid entityId)
    {
        var fileEntity = await _fileStorageRepository.GetByEntityId(entityId);

        return fileEntity ?? throw new Exception($"Файл c ID {entityId} не найден");
    }

    private static (Guid guid, string fileName) GenerateUniqueFileName(string fileNameWithExtension)
    {
        var guid = Guid.NewGuid();
        var extension = Path.GetExtension(fileNameWithExtension);
        var withoutExtension = Path.GetFileNameWithoutExtension(fileNameWithExtension);

        var pseudoFolderSuffix = GetPseudoFolderSuffix();
        var uniqueFileName = string.Concat(pseudoFolderSuffix, withoutExtension, "_", guid.ToString(), extension).Replace(" ", "_");
        return (guid, uniqueFileName);
    }

    public async Task WriteFile(string bucket, string fileName, Stream stream, string contentType)
    {
        try
        {
            var request = new PutObjectRequest
            {
                AutoCloseStream = false,
                AutoResetStreamPosition = true,
                BucketName = bucket,
                Key = fileName,
                ContentType = contentType,
                InputStream = stream
            };

            await _s3Client.PutObjectAsync(request);
        }
        catch (AmazonS3Exception e)
        {
            _logger.LogError(e, "Ошибка загрузки файла '{FileName}' в хранилище '{Bucket}'", fileName, bucket);
            throw;
        }
    }

    public async Task RemoveFile(string bucket, string fileName)
    {
        try
        {
            await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = bucket,
                Key = fileName
            });
        }
        catch (AmazonS3Exception e)
        {
            _logger.LogError(e, "Ошибка удаления файла '{FileName}' из Bucket '{Bucket}'", fileName, bucket);
            throw;
        }
    }

    public async Task RemoveFiles(FileStorageDeleteParameters deleteParameters)
    {
        if (deleteParameters.FileStorageName == null || deleteParameters.FileStorageName.Count == 0)
            return;

        var objects = deleteParameters.FileStorageName
            .Select(key => new KeyVersion
            {
                Key = key
            })
            .ToList();

        try
        {
            await _s3Client.DeleteObjectsAsync(new DeleteObjectsRequest
            {
                BucketName = deleteParameters.BucketName,
                Objects = objects
            });
        }
        catch (AmazonS3Exception e)
        {
            _logger.LogError(e, "Ошибка удаления группы файлов из хранилища '{BucketName}'", deleteParameters.BucketName);
            throw;
        }
    }

    public async Task<Stream> GetFile(string bucket, string fileName)
    {
        var request = new GetObjectRequest
        {
            BucketName = bucket,
            Key = fileName
        };
        try
        {
            using var response = await _s3Client.GetObjectAsync(request);
            await using var responseStream = response.ResponseStream;
            var ms = new MemoryStream();
            await responseStream.CopyToAsync(ms);
            return ms;
        }
        catch (AmazonS3Exception e)
        {
            _logger.LogError(e, "Ошибка получения файла '{FileName}' из хранилища '{Bucket}'", fileName, bucket);
            throw;
        }
    }

    private string GetFileUrl(string bucket, string fileName)
    {
        return $"{_s3Client.Config.ServiceURL}/{bucket}/{fileName}";
    }

    public async Task<IList<string>> GetFilesByParameters(FileStorageFilterParameters filter)
    {
        try
        {
            var result = new List<string>();

            var request = new ListObjectsV2Request
            {
                BucketName = filter.BucketName,
                MaxKeys = filter.Limit
            };

            ListObjectsV2Response response;

            do
            {
                if (result.Count >= filter.Limit)
                    break;

                response = await _s3Client.ListObjectsV2Async(request);

                var storageFiles = response.S3Objects;

                if (filter.NumberOfDaysToRemove.HasValue)
                    storageFiles = storageFiles
                        .Where(x => x.LastModified.AddDays(filter.NumberOfDaysToRemove.Value) < DateTime.UtcNow)
                        .ToList();

                var storageFilesKeys = storageFiles.Select(x => x.Key).ToList();

                result.AddRange(result.Count + storageFilesKeys.Count > filter.Limit
                    ? storageFilesKeys.SkipLast(filter.Limit - Math.Abs(result.Count - storageFilesKeys.Count))
                    : storageFilesKeys);

                request.ContinuationToken = response.NextContinuationToken;
            }
            while (response.IsTruncated);

            return result;
        }
        catch (AmazonS3Exception e)
        {
            _logger.LogError(e, "Ошибка получения списка файлов из хранилища '{BucketName}'", filter.BucketName);
            throw;
        }
    }

    public async Task CopyFile(
        string sourceBucket,
        string sourceFile,
        string destinationBucket,
        string destinationFile,
        bool removeSourceAfterCopy = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _s3Client.CopyObjectAsync(
                sourceBucket,
                sourceFile,
                destinationBucket,
                destinationFile,
                cancellationToken);

            if (removeSourceAfterCopy)
                await RemoveFile(sourceBucket, sourceFile);
        }
        catch (AmazonS3Exception e)
        {
            _logger.LogError(e, "Ошибка копирования файла {SourceFile}", sourceBucket);
            throw;
        }
    }
}