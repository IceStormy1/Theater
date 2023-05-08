using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Theater.Abstractions.TheaterWorker;
using Theater.Contracts;
using Theater.Entities.Theater;
using AutoMapper;
using Theater.Abstractions;
using Theater.Contracts.Filters;
using Theater.Controllers.BaseControllers;
using Swashbuckle.AspNetCore.Annotations;
using Theater.Abstractions.Filters;
using Theater.Contracts.Theater.TheaterWorker;
using Theater.Abstractions.FileStorage;
using Theater.Policy;

namespace Theater.Controllers;

[ApiController]
[Route("api")]
[SwaggerTag("Пользовательские методы для работы с работниками театра")]
public sealed class TheaterWorkerController : CrudServiceBaseController<TheaterWorkerParameters>
{
    private readonly ITheaterWorkerService _theaterWorkerService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IIndexReader<TheaterWorkerModel, TheaterWorkerEntity, TheaterWorkerFilterSettings> _theaterWorkerIndexReader;

    public TheaterWorkerController(
        ITheaterWorkerService theaterWorkerService,
        IMapper mapper, 
        IIndexReader<TheaterWorkerModel, TheaterWorkerEntity, TheaterWorkerFilterSettings> theaterWorkerIndexReader, 
        IFileStorageService fileStorageService) : base(theaterWorkerService, mapper)
    {
        _theaterWorkerService = theaterWorkerService;
        _theaterWorkerIndexReader = theaterWorkerIndexReader;
        _fileStorageService = fileStorageService;
    }

    /// <summary>
    /// Получить количество работников театра по каждому из типов должности 
    /// </summary>
    /// <response code="200">В случае успешного запроса</response>
    [HttpPost("workers/total")]
    [ProducesResponseType(typeof(TotalWorkersModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTotalWorkers()
    {
        var totalWorkers = await _theaterWorkerService.GetTotalWorkers();

        return Ok(totalWorkers);
    }

    /// <summary>
    /// Получить краткую информацию о работниках театра по типу должности
    /// </summary>
    /// <remarks>
    /// Доступна сортировка по полям:
    /// * name
    /// * position
    /// </remarks>
    /// <response code="200">В случае успешного запроса</response>
    [HttpGet("workers")]
    [ProducesResponseType(typeof(DocumentCollection<TheaterWorkerShortInformationModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetShortInformationWorkersByPositionType([FromQuery] TheaterWorkerFilterParameters filter)
    {
        var filterSettings = Mapper.Map<TheaterWorkerFilterSettings>(filter);
        var theaterWorkers = await _theaterWorkerIndexReader.QueryItems(filterSettings);
        var theaterWorkerShortInformation = Mapper.Map<Page<TheaterWorkerShortInformationModel>>(theaterWorkers);

        await _theaterWorkerService.EnrichTheaterWorkerShortInfo(theaterWorkerShortInformation);

        return Ok(Mapper.Map<Page<TheaterWorkerShortInformationModel>>(theaterWorkers));
    }

    /// <summary>
    /// Получить полную информацию о работнике театра по его идентификатору
    /// </summary>
    /// <response code="200">В случае успешного запроса</response>
    /// <response code="404">В случае успешного запроса</response>
    [HttpGet("worker/{theaterWorkerId:guid}")]
    [ProducesResponseType(typeof(TheaterWorkerModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTheaterWorkerById([FromRoute] Guid theaterWorkerId)
    {
        var theaterWorker = await _theaterWorkerIndexReader.GetById(theaterWorkerId);
        
        if(theaterWorker.IsSuccess && theaterWorker.ResultData.MainPhoto != null)
            theaterWorker.ResultData.MainPhoto = await _fileStorageService.GetStorageFileInfoById(theaterWorker.ResultData.MainPhoto.Id);

        return RenderResult(theaterWorker);
    }
}