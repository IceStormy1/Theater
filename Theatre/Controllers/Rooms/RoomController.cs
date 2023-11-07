using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using Theater.Abstractions.Errors;
using Theater.Abstractions.Rooms;
using Theater.Contracts.Messages;
using Theater.Controllers.Base;

namespace Theater.Controllers.Rooms;

[SwaggerTag("Пользовательские методы для работы с чатом")]
[Authorize]
[Route("api")]
public class RoomController : CrudServiceBaseController<RoomParameters>
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService service, IMapper mapper) : base(service, mapper)
    {
        _roomService = service;
    }

    /// <summary>
    /// Создает новую комнату
    /// </summary>
    /// <param name="roomParameters">Тело нового сообщения</param>
    [HttpPost("rooms")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> CreateRoom([FromBody] RoomParameters roomParameters)
    {
        return !UserId.HasValue
            ? RenderResult(UserAccountErrors.Unauthorized)
            : RenderResult(await _roomService.CreateOrUpdate(roomParameters, entityId: null, userId: UserId.Value));
    }

    /// <summary>
    /// Обновляет комнату
    /// </summary>
    /// <param name="roomId">Идентификатор комнаты</param>
    /// <param name="roomParameters">Тело нового сообщения</param>
    [HttpPut("rooms/{roomId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateRoom([FromRoute] Guid roomId, [FromBody] RoomParameters roomParameters)
    {
        return !UserId.HasValue
            ? RenderResult(UserAccountErrors.Unauthorized)
            : RenderResult(await _roomService.CreateOrUpdate(roomParameters, roomId, userId: UserId.Value));
    }

    /// <summary>
    /// Обновляет комнату
    /// </summary>
    /// <param name="roomId">Идентификатор комнаты</param>
    [HttpDelete("rooms/{roomId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteRoom([FromRoute] Guid roomId)
    {
        return !UserId.HasValue
            ? RenderResult(UserAccountErrors.Unauthorized)
            : RenderResult(await _roomService.Delete(roomId, userId: UserId.Value));
    }
}