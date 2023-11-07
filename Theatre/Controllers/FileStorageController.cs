using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using Theater.Abstractions.FileStorage;
using Theater.Common.Enums;
using Theater.Contracts.FileStorage;
using Theater.Controllers.Base;

namespace Theater.Controllers;

[SwaggerTag("Пользовательские методы для работы с файловым хранилищем")]
public class FileStorageController : BaseController
{
    private readonly IFileStorageService _fileStorageService;

    public FileStorageController(
        IMapper mapper,
        IFileStorageService fileStorageService) : base(mapper)
    {
        _fileStorageService = fileStorageService;
    }

    /// <summary>
    /// Загрузить файл в файловое хранилище
    /// </summary>
    /// <param name="bucketId">Идентификатор бакета</param>
    /// <param name="file">Файл</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(StorageFileListItem), StatusCodes.Status200OK)]
    public async Task<StorageFileListItem> Upload([FromQuery] BucketIdentifier bucketId,  IFormFile file)
    {
        await using var stream = file.OpenReadStream();

        return await _fileStorageService.Upload(bucketId, stream, file.FileName);
    }

    /// <summary>
    /// Получить ссылку на файл по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор файла</param>
    /// <returns></returns>
    [HttpGet("{id:guid}/url")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUrlById([FromRoute] Guid id)
    {
        var fileUrl = await _fileStorageService.GetUrlById(id);

        return !string.IsNullOrWhiteSpace(fileUrl)
            ? Ok(fileUrl)
            : NotFound();
    }

    /// <summary>
    /// Получить полную информацию о файле (метаданные)
    /// </summary>
    /// <param name="id">Идентификатор файла</param>
    /// <returns></returns>
    [HttpGet("{id:guid}/meta")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(StorageFileInfo), StatusCodes.Status200OK)]
    public Task<StorageFileInfo> GetStorageFileInfo([FromRoute] Guid id)
    {
        return _fileStorageService.GetStorageFileInfoById(id);
    }

    /// <summary>
    /// Получить сам файл по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор файла</param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    public async Task<FileStreamResult> GetFileById([FromRoute] Guid id)
    {
        var storageFileInfo = await _fileStorageService.GetStorageFileInfoById(id);
        var stream = await _fileStorageService.GetFileStreamById(id, storageFileInfo.Bucket, storageFileInfo.StorageFileName);
        return File(stream, storageFileInfo.ContentType, storageFileInfo.FileName);
    }

    /// <summary>
    /// Удалить файл по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор файла</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteFileById([FromRoute] Guid id)
    {
        await _fileStorageService.DeleteFileById(id);
        return Ok();
    }
}