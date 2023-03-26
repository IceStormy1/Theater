﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Theater.Abstractions.TheaterWorker;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Controllers.Admin
{
    [Authorize]
    [Route("api/admin/theaterWorker")]
    public sealed class TheaterWorkerAdminController : BaseAdminController<ITheaterWorkerService, TheaterWorkerParameters, TheaterWorkerEntity>
    {
        public TheaterWorkerAdminController(ITheaterWorkerService service) : base(service)
        {
        }
    }
}
