﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Theater.Abstractions.FileStorage;
using Theater.Common;
using Theater.Contracts.FileStorage;
using Theater.Controllers.BaseControllers;

namespace Theater.Controllers
{
    [ApiController]
    public class FileStorageController : TheaterBaseController
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
        [HttpPost("{bucketId:int}/upload")]
        [ProducesResponseType(typeof(StorageFileListItem), StatusCodes.Status200OK)]
        public async Task<StorageFileListItem> Upload([FromRoute] int bucketId,  IFormFile file)
        {
            await using var stream = file.OpenReadStream();

            return await _fileStorageService.Upload((BucketIdentifier)bucketId, stream, file.FileName);
        }

        /// <summary>
        /// Получить ссылку на файл по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор файла</param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
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
        /// Получить полную информацию о файле
        /// </summary>
        /// <param name="id">Идентификатор файла</param>
        /// <returns></returns>
        [HttpGet("getstoragefileinfo/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(StorageFileInfo), StatusCodes.Status200OK)]
        public Task<StorageFileInfo> GetStorageFileInfo([FromRoute] Guid id)
        {
            return _fileStorageService.GetStorageFileInfo(id);
        }

        /// <summary>
        /// Получить файл по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор файла</param>
        /// <returns></returns>
        [HttpGet("getfilebyid/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        public async Task<FileStreamResult> GetFileById([FromRoute] Guid id)
        {
            var storageFileInfo = await _fileStorageService.GetStorageFileInfo(id);
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
}
