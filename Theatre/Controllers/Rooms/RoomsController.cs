using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions.Errors;
using Theater.Abstractions.Rooms;
using Theater.Abstractions.UserAccount;
using Theater.Contracts.Filters;
using Theater.Contracts.Messages;
using Theater.Contracts.Rooms;
using Theater.Controllers.Base;

namespace Theater.Controllers.Rooms;

[Tags("Rooms")]
[SwaggerTag("Пользовательские методы для работы с чатом")]
[Authorize]
public sealed class RoomsController : CrudServiceBaseController<RoomParameters>
{
    private readonly IRoomService _roomService;

    public RoomsController(
        IRoomService service, 
        IMapper mapper,
        IUserAccountService userAccountService
        ) : base(service, mapper, userAccountService)
    {
        _roomService = service;
    }

    /// <summary>
    /// Получение списка чатов с фильтрацией
    /// </summary>
    /// <param name="filter">Фильтр для поиска чатов/контактов</param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RoomItemDto>))]
    public async Task<IActionResult> GetRooms([FromQuery] RoomSearchParameters filter)
    {
        var innerUserId = await GetUserId();

        return !innerUserId.HasValue
            ? RenderResult(UserAccountErrors.Unauthorized)
            : RenderResult(await _roomService.GetRoomsForUser(innerUserId.Value, filter));
    }

    /// <summary>
    /// Создает новую комнату
    /// </summary>
    /// <param name="roomParameters">Тело нового сообщения</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> CreateRoom([FromBody] RoomParameters roomParameters)
    {
        var innerUserId = await GetUserId();

        return !innerUserId.HasValue
            ? RenderResult(UserAccountErrors.Unauthorized)
            : RenderResult(await _roomService.CreateOrUpdate(roomParameters, entityId: null, userId: innerUserId.Value));
    }

    /// <summary>
    /// Обновляет комнату
    /// </summary>
    /// <param name="roomId">Идентификатор комнаты</param>
    /// <param name="roomParameters">Тело нового сообщения</param>
    [HttpPut("{roomId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateRoom([FromRoute] Guid roomId, [FromBody] RoomParameters roomParameters)
    {
        var innerUserId = await GetUserId();

        return !innerUserId.HasValue
            ? RenderResult(UserAccountErrors.Unauthorized)
            : RenderResult(await _roomService.CreateOrUpdate(roomParameters, roomId, userId: innerUserId.Value));
    }
}