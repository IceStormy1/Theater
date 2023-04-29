using AutoMapper;
using Swashbuckle.AspNetCore.Annotations;
using Theater.Abstractions.WorkersPosition;
using Theater.Contracts.Theater;
using Theater.Controllers.BaseControllers;
using Theater.Entities.Theater;

namespace Theater.Controllers.Admin;

[SwaggerTag("Админ. Методы для работы с должностями работников театра")]
public class WorkersPositionAdminController : AdminBaseController<WorkersPositionParameters, WorkersPositionEntity>
{
    public WorkersPositionAdminController(
        IWorkersPositionService service,
        IMapper mapper) : base(service, mapper)
    {
    }
}