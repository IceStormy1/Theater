using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Theater.Abstractions;

namespace Theater.Controllers.Base;

/// <summary>
/// Базовый контроллер с <see cref="ICrudService{TParameters}"/>. Путь по умолчанию: <c>api/[controller]</c>.
/// </summary>
[ApiController]
public class CrudServiceBaseController<TParameters> : BaseController
    where TParameters : class
{
    protected readonly ICrudService<TParameters> Service;

    public CrudServiceBaseController(
        ICrudService<TParameters> service,
        IMapper mapper) : base(mapper) 
    {
        Service = service;
    }
}