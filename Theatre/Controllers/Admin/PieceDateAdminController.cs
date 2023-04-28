using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Theater.Abstractions.PieceDates;
using Theater.Contracts.Theater;
using Theater.Controllers.BaseControllers;
using Theater.Entities.Theater;

namespace Theater.Controllers.Admin;

[Route("api/admin/piece/date")]
public class PieceDateAdminController : AdminBaseController<PieceDateParameters, PieceDateEntity>
{
    public PieceDateAdminController(
        IPieceDateService service, 
        IMapper mapper) : base(service, mapper)
    {
    }
}