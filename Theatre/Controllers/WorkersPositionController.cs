using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using Theater.Abstractions.UserAccount;
using Theater.Abstractions.WorkersPosition;
using Theater.Common.Enums;
using Theater.Contracts;
using Theater.Contracts.Theater.WorkersPosition;
using Theater.Controllers.Base;

namespace Theater.Controllers;

[Route("api")]
[SwaggerTag("Пользовательские методы для работы с должностями работников театра")]
public sealed class WorkersPositionController : CrudServiceBaseController<WorkersPositionParameters>
{
    private readonly IWorkersPositionService _workersPositionService;

    public WorkersPositionController(
        IWorkersPositionService workersPositionService,
        IMapper mapper,
        IUserAccountService userAccountService) : base(workersPositionService, mapper, userAccountService)
    {
        _workersPositionService = workersPositionService;
    }

    /// <summary>
    /// Возвращает все должности работников театра
    /// </summary>
    /// <param name="positionType">
    /// Тип должности. Опциональный параметр
    /// </param>
    [HttpGet("positions")]
    [ProducesResponseType(typeof(DocumentCollection<WorkersPositionModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPieceTicketsByDate([FromQuery] PositionType? positionType)
    {
        var tickets = await _workersPositionService.GetWorkerPositions(positionType);

        return Ok(new DocumentCollection<WorkersPositionModel>(tickets));
    }
}