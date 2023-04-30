using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using Theater.Abstractions.Errors;
using Theater.Abstractions.UserReviews;
using Theater.Contracts.Theater.UserReview;
using Theater.Contracts.UserAccount;
using Theater.Controllers.BaseControllers;

namespace Theater.Controllers;

[ApiController]
[Route("api/review")]
[Authorize]
[SwaggerTag("Пользовательские методы для работы с рецензиями")]
public sealed class UserReviewsController : CrudServiceBaseController<UserReviewParameters>
{
    private readonly IUserReviewsService _userReviewsService;

    public UserReviewsController(IUserReviewsService service, IMapper mapper) : base(service, mapper)
    {
        _userReviewsService = service;
    }

    /// <summary>
    /// Добавить рецензию пользователя
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateReview([FromBody] UserReviewParameters parameters)
    {
        if (!UserId.HasValue)
            return RenderResult(UserAccountErrors.Unauthorized);

        parameters.UserId = UserId.Value;
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
        if (!UserId.HasValue)
            return RenderResult(UserAccountErrors.Unauthorized);

        parameters.UserId = UserId.Value;
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
        var result = await _userReviewsService.Delete(id, UserId);

        return RenderResult(result);
    }
}