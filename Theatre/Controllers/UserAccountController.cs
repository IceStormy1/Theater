using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Errors;
using Theater.Abstractions.FileStorage;
using Theater.Abstractions.Filters;
using Theater.Abstractions.UserAccount;
using Theater.Contracts;
using Theater.Contracts.Authorization;
using Theater.Contracts.Filters;
using Theater.Contracts.Theater.PurchasedUserTicket;
using Theater.Contracts.UserAccount;
using Theater.Controllers.BaseControllers;
using Theater.Entities.Authorization;
using Theater.Entities.Theater;
using VkNet.Abstractions;
using RoleUser = Theater.Abstractions.Authorization.Models.UserRole;

namespace Theater.Controllers;

[ApiController]
[Route("api/account")]
[Authorize]
[SwaggerTag("Пользовательские методы для работы с аккаунтом")]
public sealed class UserAccountController : CrudServiceBaseController<UserParameters>
{
    private readonly IUserAccountService _userAccountService;
    private readonly IIndexReader<UserModel, UserEntity, UserAccountFilterSettings> _userIndexReader;
    private readonly IIndexReader<PurchasedUserTicketModel, PurchasedUserTicketEntity, PieceTicketFilterSettings> _pieceTicketIndexReader;
    private readonly IFileStorageService _fileStorageService;
    private readonly IVkApi _vkApiAuth;

    public UserAccountController(
        IUserAccountService userAccountService,
        IMapper mapper,
        IIndexReader<UserModel, UserEntity, UserAccountFilterSettings> userIndexReader,
        IIndexReader<PurchasedUserTicketModel, PurchasedUserTicketEntity, PieceTicketFilterSettings> pieceTicketIndexReader,
        IFileStorageService fileStorageService,
        IVkApi vkApiAuth) : base(userAccountService, mapper)
    {
        _userAccountService = userAccountService;
        _userIndexReader = userIndexReader;
        _fileStorageService = fileStorageService;
        _vkApiAuth = vkApiAuth;
        _pieceTicketIndexReader = pieceTicketIndexReader;
    }

    /// <summary>
    /// Регистрирует нового пользователя
    /// </summary>
    /// <response code="200">В случае успешной регистрации</response>
    /// <response code="400">В случае ошибок валидации</response>
    [AllowAnonymous]
    [HttpPost("registration")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registration([FromBody] UserParameters parameters)
    {
        var createUserResult = await _userAccountService.CreateUser(parameters);

        return RenderResult(createUserResult);
    }

    /// <summary>
    /// Возвращает пользователя по идентификатору
    /// </summary>
    /// <returns></returns>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <response code="200">В случае, если пользователь был найден в системе</response>
    /// <response code="404">В случае если пользователь не был найден</response>
    [AllowAnonymous]
    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
    {
        var user = await _userIndexReader.GetById(userId);

        if(user.IsSuccess && user.ResultData.Photo != null)
            user.ResultData.Photo = await _fileStorageService.GetStorageFileInfoById(user.ResultData.Photo.Id);

        return RenderResult(user);
    }

    /// <summary>
    /// Возвращает купленные билеты пользователя 
    /// </summary>
    /// <remarks>
    /// Доступна сортировка по полям:
    /// * pieceName
    /// * pieceDate
    /// * dateOfPurchase
    /// </remarks>
    /// <param name="filterParameters">Параметры запроса</param>
    /// <response code="200">В случае успешного запроса</response>
    /// <response code="400">В случае ошибок валидации</response>
    [HttpGet("user/tickets")]
    [ProducesResponseType(typeof(Page<PurchasedUserTicketModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserTickets([FromQuery] PieceTicketFilterParameters filterParameters)
    {
        if (!UserId.HasValue)
            return RenderResult(UserAccountErrors.Unauthorized);

        if (UserId.Value != filterParameters.UserId && UserRole != RoleUser.Admin)
            return RenderResult(UserAccountErrors.InsufficientRights);

        var filterSettings = Mapper.Map<PieceTicketFilterSettings>(filterParameters);
        var tickets = await _pieceTicketIndexReader.QueryItems(filterSettings);

        return Ok(Mapper.Map<Page<PurchasedUserTicketModel>>(tickets));
    }

    /// <summary>
    /// Войти при помощи логина и пароля
    /// </summary>
    /// <response code="200">В случае успешной регистрации</response>
    /// <response code="400">В случае ошибок валидации</response>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] AuthenticateParameters parameters)
    {
        var authenticateResult = await _userAccountService.Authorize(parameters);       

        return authenticateResult is null
            ? RenderResult(UserAccountErrors.NotFound)
            : Ok(authenticateResult);
    }

    /// <summary>
    /// Войти при помощи логина и пароля
    /// </summary>
    /// <response code="200">В случае успешной регистрации</response>
    /// <response code="400">В случае ошибок валидации</response>
    [AllowAnonymous]
    [HttpPost("vk/login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginWithVkToken([FromBody] AuthenticateVkDto parameters)
    {
        var authenticateResult = await _userAccountService.AuthorizeWithVk(parameters);

        return RenderResult(authenticateResult);
    }

    /// <summary>
    /// Обновить профиль пользователя
    /// </summary>
    /// <response code="200">В случае успешного запроса</response>
    /// <response code="400">В случае ошибок валидации</response>
    [HttpPut("{userId:guid}/update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, [FromBody] UserParameters parameters)
    {
        if (!UserId.HasValue)
            return RenderResult(UserAccountErrors.Unauthorized);

        if (UserId.Value != userId && UserRole != RoleUser.Admin)
            return RenderResult(UserAccountErrors.InsufficientRights);

        var updateResult = await _userAccountService.UpdateUser(parameters, userId);

        return RenderResult(updateResult);
    }

    /// <summary>
    /// Пополнить баланс пользователя
    /// </summary>
    /// <response code="200">В случае успешного запроса</response>
    /// <response code="400">В случае ошибок валидации</response>
    [HttpPatch("replenish")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReplenishBalance([FromBody] UserReplenishParameters parameters)
    {
        if (!UserId.HasValue)
            return RenderResult(UserAccountErrors.Unauthorized);

        var replenishResult = await _userAccountService.ReplenishBalance(UserId.Value, parameters.ReplenishmentAmount);

        return RenderResult(replenishResult);
    }
}