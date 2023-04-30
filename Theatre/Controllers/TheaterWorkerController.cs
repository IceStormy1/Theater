using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Theater.Abstractions.TheaterWorker;
using Theater.Contracts;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;
using AutoMapper;
using Theater.Abstractions;
using Theater.Abstractions.Filter;
using Theater.Contracts.Filters;
using Theater.Controllers.BaseControllers;
using Swashbuckle.AspNetCore.Annotations;

namespace Theater.Controllers;

[ApiController]
[Route("api")]
[SwaggerTag("Пользовательские методы для работы с работниками театра")]
public sealed class TheaterWorkerController : CrudServiceBaseController<TheaterWorkerParameters>
{
    private readonly ITheaterWorkerService _theaterWorkerService;
    private readonly IIndexReader<TheaterWorkerModel, TheaterWorkerEntity, TheaterWorkerFilterSettings> _theaterWorkerIndexReader;

    public TheaterWorkerController(
        ITheaterWorkerService theaterWorkerService,
        IMapper mapper, 
        IIndexReader<TheaterWorkerModel, TheaterWorkerEntity, TheaterWorkerFilterSettings> theaterWorkerIndexReader) : base(theaterWorkerService, mapper)
    {
        _theaterWorkerService = theaterWorkerService;
        _theaterWorkerIndexReader = theaterWorkerIndexReader;
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
        var workersShortInformation = await _theaterWorkerIndexReader.QueryItems(filterSettings);

        return Ok(Mapper.Map<Page<TheaterWorkerShortInformationModel>>(workersShortInformation));
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

        return RenderResult(theaterWorker);
    }
}