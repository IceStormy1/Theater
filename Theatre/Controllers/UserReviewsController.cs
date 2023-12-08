using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Errors;
using Theater.Abstractions.Filters;
using Theater.Abstractions.UserAccount;
using Theater.Abstractions.UserReviews;
using Theater.Contracts;
using Theater.Contracts.Filters;
using Theater.Contracts.Theater.UserReview;
using Theater.Contracts.UserAccount;
using Theater.Controllers.Base;
using Theater.Entities.Theater;

namespace Theater.Controllers;

[ApiController]
[Route("api/user/review")]
[Authorize]
[SwaggerTag("Пользовательские методы для работы с рецензиями")]
public sealed class UserReviewsController : CrudServiceBaseController<UserReviewParameters>
{
    private readonly IUserReviewsService _userReviewsService;
    private readonly IIndexReader<UserReviewModel, UserReviewEntity, UserReviewFilterSettings> _userReviewIndexReader;

    public UserReviewsController(
        IUserReviewsService service, 
        IMapper mapper, 
        IIndexReader<UserReviewModel, UserReviewEntity, UserReviewFilterSettings> userReviewIndexReader,
        IUserAccountService userAccountService
        ) : base(service, mapper, userAccountService)
    {
        _userReviewsService = service;
        _userReviewIndexReader = userReviewIndexReader;
    }

    /// <summary>
    /// Добавить рецензию пользователя
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateReview([FromBody] UserReviewParameters parameters)
    {
        var innerUserId = await GetUserId();

        if (!innerUserId.HasValue)
            return RenderResult(UserAccountErrors.Unauthorized);

        parameters.UserId = innerUserId.Value;
        var result = await _userReviewsService.CreateOrUpdate(parameters, null);

        return RenderResult(result);
    }

    /// <summary>
    /// Редактировать рецензию пользователя
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateReview([FromRoute] Guid id, [FromBody] UserReviewParameters parameters)
    {
        var innerUserId = await GetUserId();

        if (!innerUserId.HasValue)
            return RenderResult(UserAccountErrors.Unauthorized);

        parameters.UserId = innerUserId.Value;
        var result = await _userReviewsService.CreateOrUpdate(parameters, id);

        return RenderResult(result);
    }

    /// <summary>
    /// Удалить рецензию пользователя
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReview([FromRoute] Guid id)
    {
        var innerUserId = await GetUserId();
        var result = await _userReviewsService.Delete(id, innerUserId);

        return RenderResult(result);
    }

    /// <summary>
    /// Возвращает отзывы пользователя по параметрам
    /// </summary>
    /// <remarks>
    /// Доступна сортировка по следующим полям:
    /// * title
    /// * pieceName
    /// * userName
    /// </remarks>
    /// <param name="filterParameters">Параметры запроса</param>
    /// <response code="200">В случае успешного запроса</response>
    [AllowAnonymous]
    [HttpGet("list")]
    [ProducesResponseType(typeof(Page<UserReviewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserReviews([FromQuery] UserReviewFilterParameters filterParameters)
    {
        var filterSettings = Mapper.Map<UserReviewFilterSettings>(filterParameters);
        var reviews = await _userReviewIndexReader.QueryItems(filterSettings);

        return Ok(Mapper.Map<Page<UserReviewModel>>(reviews));
    }
}