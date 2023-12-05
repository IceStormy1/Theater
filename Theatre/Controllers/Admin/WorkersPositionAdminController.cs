using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using Theater.Abstractions.UserAccount;
using Theater.Abstractions.WorkersPosition;
using Theater.Common.Enums;
using Theater.Common.Extensions;
using Theater.Contracts;
using Theater.Contracts.Theater.WorkersPosition;
using Theater.Controllers.Base;

namespace Theater.Controllers.Admin;

[SwaggerTag("Админ. Методы для работы с должностями работников театра")]
[Route("api/admin/position")]
public sealed class WorkersPositionAdminController : AdminBaseController<WorkersPositionParameters>
{
    public WorkersPositionAdminController(
        IWorkersPositionService service,
        IMapper mapper,
        IUserAccountService userAccountService
        ) : base(service, mapper, userAccountService)
    {
    }

    /// <summary>
    /// Возвращает доступные типы должности
    /// </summary>
    [HttpGet("types")]
    [ProducesResponseType(typeof(DocumentCollection<PositionTypeItem>), StatusCodes.Status200OK)]
    public IActionResult GetPositionTypes()
    {
        var workerPositions = Enum.GetValues(typeof(PositionType))
            .Cast<PositionType>()
            .Select(t => new PositionTypeItem
            {
                Id = (ushort)t, 
                DisplayName = t.GetEnumDisplayName(), 
                Name = t.ToString()
            })
            .ToList();

        return Ok(new DocumentCollection<PositionTypeItem>(workerPositions));

    }
}