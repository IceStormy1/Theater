using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Theater.Abstractions.Errors;
using Theater.Abstractions.Rooms;
using Theater.Contracts.Messages;
using Theater.Contracts.Rooms;
using Theater.Controllers.Base;

namespace Theater.Controllers.Rooms;

[Tags("Rooms")]
[Authorize]
[Route("api/rooms/{roomId:guid}/users")]
public sealed class UserRoomsController : BaseController
{
    private readonly IUserRoomService _userRoomService;

    public UserRoomsController(
        IMapper mapper, 
        IUserRoomService userRoomService
        ) : base(mapper)
    {
        _userRoomService = userRoomService;
    }

    /// <summary>
    /// Покинуть комнату
    /// </summary>
    /// <param name="roomId">Идентификатор комнаты</param>
    [HttpDelete("leave")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> LeaveRoom([FromRoute] Guid roomId)
    {
        return !UserId.HasValue
            ? RenderResult(UserAccountErrors.Unauthorized)
            : RenderResult(await _userRoomService.LeaveRoom(userId: UserId.Value, roomId));
    }

    /// <summary>
    /// Добавляет пользователей в комнату
    /// </summary>
    /// <param name="roomId">Идентификатор комнаты</param>
    /// <param name="inviteUsersModel">Тело</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> InviteUsersToRoom([FromRoute] Guid roomId, [FromBody] InviteUsersModel inviteUsersModel)
    {
        return !UserId.HasValue
            ? RenderResult(UserAccountErrors.Unauthorized)
            : RenderResult(await _userRoomService.InviteUsersToRoom(UserId.Value, roomId, inviteUsersModel));
    }

    /// <summary>
    /// Пометить сообщение как прочитанное
    /// </summary>
    /// <param name="roomId">Идентификатор комнаты</param>
    /// <param name="messageId">Идентификатор сообщения</param>
    [HttpPost("messages/{messageId:guid}/read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ReadMessage([FromRoute] Guid roomId, [FromRoute] Guid messageId)
    {
        return !UserId.HasValue
            ? RenderResult(UserAccountErrors.Unauthorized)
            : RenderResult(await _userRoomService.ReadMessage(UserId.Value, roomId, messageId));
    }
}