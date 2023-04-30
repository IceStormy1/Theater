﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Theater.Abstractions.PieceDates;
using Theater.Contracts.Theater.PieceDate;
using Theater.Controllers.BaseControllers;

namespace Theater.Controllers.Admin;

[Route("api/admin/piece/date")]
[SwaggerTag("Админ. Методы для работы с датами пьес")]
public class PieceDateAdminController : AdminBaseController<PieceDateParameters>
{
    public PieceDateAdminController(
        IPieceDateService service, 
        IMapper mapper) : base(service, mapper)
    {
    }
}