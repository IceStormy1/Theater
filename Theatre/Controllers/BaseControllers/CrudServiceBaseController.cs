using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Theater.Abstractions;
using Theater.Entities;

namespace Theater.Controllers.BaseControllers
{
    /// <summary>
    /// Базовый контроллер с <see cref="ICrudService{TParameters, TEntity}"/>. Путь по умолчанию: <c>api/[controller]</c>.
    /// </summary>
    [ApiController]
    public class CrudServiceBaseController<TParameters, TEntity> : TheaterBaseController
        where TParameters : class
        where TEntity : class, IEntity
    {
        protected readonly ICrudService<TParameters, TEntity> Service;

        public CrudServiceBaseController(
            ICrudService<TParameters, TEntity> service,
            IMapper mapper) : base(mapper) 
        {
            Service = service;
        }
    }
}
