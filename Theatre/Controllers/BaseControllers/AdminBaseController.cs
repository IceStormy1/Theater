using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Theater.Abstractions;
using Theater.Contracts;

namespace Theater.Controllers.BaseControllers;

/// <summary>
/// Базовый контроллер в админке с реализацией CRUD. Путь по умолчанию: <c>api/admin</c>.
/// </summary>
[Authorize] 
[Route("api/admin")] 
[ApiController]
public class AdminBaseController<TParameters> : CrudServiceBaseController<TParameters>
    where TParameters : class
{
    public AdminBaseController(
        ICrudService<TParameters> service,
        IMapper mapper) : base(service, mapper)
    {
    }

    /// <summary>
    /// Создать сущность
    /// </summary>
    /// <response code="200">В случае успешного запроса</response>
    /// <response code="400">В случае ошибок валидации</response>
    // [Authorize(Policy = nameof(RoleModel.User.Policies.UserSearch))]
    [HttpPost]
    [ProducesResponseType(typeof(DocumentMeta), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] TParameters parameters)
        => await CreateOrUpdate(parameters);

    /// <summary>
    /// Обновить сущность
    /// </summary>
    /// <response code="200">В случае успешного запроса</response>
    /// <response code="400">В случае ошибок валидации</response>
    // [Authorize(Policy = nameof(RoleModel.User.Policies.UserSearch))]
    [HttpPut("{entityId:guid}")]
    [ProducesResponseType(typeof(DocumentMeta), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] TParameters parameters, [FromRoute] Guid entityId)
        => await CreateOrUpdate(parameters, entityId);

    /// <summary>
    /// Удалить сущность
    /// </summary>
    /// <response code="200">В случае успешного запроса</response>
    /// <response code="400">В случае ошибок валидации</response>
    // [Authorize(Policy = nameof(RoleModel.User.Policies.UserSearch))]
    [HttpDelete("{entityId:guid}")]
    [ProducesResponseType(typeof(DocumentMeta), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromRoute] Guid entityId)
    {
        var deletePieceResult = await Service.Delete(entityId);

        return RenderResult(deletePieceResult);
    }

    /// <summary>
    /// Обновить или создать сущность
    /// </summary>
    /// <param name="parameters">Параметры</param>
    /// <param name="entityId">Идентификатор сущности</param>
    /// <remarks>
    /// Идентификатор <paramref name="entityId"/> указывается при обновлении сущности
    /// </remarks>
    private async Task<IActionResult> CreateOrUpdate(TParameters parameters, Guid? entityId = null)
    {
        var piecesShortInformation = await Service.CreateOrUpdate(parameters, entityId);

        return RenderResult(piecesShortInformation);
    }
}