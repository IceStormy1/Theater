using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Theater.Abstractions.PieceDates;
using Theater.Contracts.Theater;
using Theater.Controllers.BaseControllers;
using Theater.Entities.Theater;

namespace Theater.Controllers.Admin;

[Route("api/admin/piece/date")]
[SwaggerTag("Админ. Методы для работы с датами пьес")]
public class PieceDateAdminController : AdminBaseController<PieceDateParameters, PieceDateEntity>
{
    public PieceDateAdminController(
        IPieceDateService service, 
        IMapper mapper) : base(service, mapper)
    {
    }
}