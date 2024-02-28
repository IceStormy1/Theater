using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Theater.Abstractions.UserAccount;
using Theater.Common;
using Theater.Common.Enums;
using Theater.Common.Extensions;

namespace Theater.Controllers.Base;

/// <summary>
/// Базовый контроллер. Путь по умолчанию: <c>api/[controller]</c>.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    /// <summary>
    /// Роль пользователя
    /// </summary>
    protected UserRole UserRole => GetUserRoleFromToken();

    /// <summary>
    /// ExternalId авторизованного пользователя
    /// </summary>
    protected Guid AuthorizedUserExternalId
    {
        get
        {
            var nameIdentifier = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            return !string.IsNullOrWhiteSpace(nameIdentifier) && Guid.TryParse(nameIdentifier, out var userExternalId)
                ? userExternalId
                : default;
        }
    }

    protected readonly IMapper Mapper;

    private readonly IUserAccountService _userAccountService;

    public BaseController(
        IMapper mapper, 
        IUserAccountService userAccountService)
    {
        Mapper = mapper;
        _userAccountService = userAccountService;
    }

    /// <summary>
    /// Возвращает ActionResult из <see cref="IResult{T}"/>
    /// </summary>
    /// <param name="source">Result с ошибкой или моделью</param>
    /// <typeparam name="T"></typeparam>
    protected IActionResult RenderResult<T>(IResult<T> source)
    {
        if (!source.IsSuccess)
            return RenderError(source.Error);

        return source.ResultData == null 
            ? new NoContentResult()
            : new JsonResult(source.ResultData);
    }

    /// <summary>
    /// Возвращает ActionResult из Result
    /// </summary>
    /// <param name="source">Result</param>
    protected IActionResult RenderResult(Result source)
        => source.IsSuccess ? new OkResult() : RenderError(source.Error);

    /// <summary>
    /// Получить внутренний идентификатор пользователя по идентификатору из токена
    /// </summary>
    /// <returns></returns>
    protected Task<Guid?> GetUserId()
        => AuthorizedUserExternalId == Guid.Empty
            ? null
            : _userAccountService.GetUserIdByExternalId(AuthorizedUserExternalId);

    /// <summary>
    /// Возвращает action result для ошибки как <see cref="ProblemDetails"/> с status code
    /// </summary>
    /// <param name="errorModel">Error model</param>
    private IActionResult RenderError(ErrorModel errorModel)
    {
        var statusCode = GetStatusCode(errorModel.Kind);
        var problemDetails = new ProblemDetails
        {
            Type = errorModel.Type,
            Title = errorModel.Message,
            Instance = Request.Path,
            Status = statusCode
        };

        if (errorModel.Errors is { Count: > 0 })
            problemDetails.Extensions[nameof(errorModel.Errors)] = errorModel.Errors;

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    private static int GetStatusCode(ErrorKind errorKind)
    {
        return errorKind switch
        {
            ErrorKind.Forbidden => StatusCodes.Status403Forbidden,
            ErrorKind.NotFound => StatusCodes.Status404NotFound,
            ErrorKind.Default => StatusCodes.Status400BadRequest,
            ErrorKind.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => throw new ArgumentOutOfRangeException(nameof(errorKind), errorKind, null)
        };
    }

    /// <summary>
    /// Получить роль пользователя из токена
    /// </summary>
    private UserRole GetUserRoleFromToken()
        => User.Claims.First(x => x.Type == "role").Value
            .ConvertStringToEnum<UserRole>();
}