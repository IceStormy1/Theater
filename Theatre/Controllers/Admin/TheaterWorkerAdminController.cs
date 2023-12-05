using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Theater.Abstractions.TheaterWorker;
using Theater.Abstractions.UserAccount;
using Theater.Contracts.Theater.TheaterWorker;
using Theater.Controllers.Base;

namespace Theater.Controllers.Admin;

[Route("api/admin/theaterWorker")]
[SwaggerTag("Админ. Методы для работы работниками театра")]
public sealed class TheaterWorkerAdminController : AdminBaseController<TheaterWorkerParameters>
{
    public TheaterWorkerAdminController(
        ITheaterWorkerService service,
        IMapper mapper
        , IUserAccountService userAccountService
        ) : base(service, mapper, userAccountService)
    {
    }
}