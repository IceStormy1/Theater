using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Theater.Abstractions.TheaterWorker;
using Theater.Contracts.Theater;
using Theater.Controllers.BaseControllers;
using Theater.Entities.Theater;

namespace Theater.Controllers.Admin
{
    [Route("api/admin/theaterWorker")]
    public sealed class TheaterWorkerAdminController : AdminBaseController<TheaterWorkerParameters, TheaterWorkerEntity>
    {
        public TheaterWorkerAdminController(
            ITheaterWorkerService service,
            IMapper mapper) : base(service, mapper)
        {
        }
    }
}
