using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Theater.Abstractions.UserAccount.Models;
using Theater.Contracts.Authorization;
using Theater.Policy;
using IAuthorizationService = Theater.Abstractions.Authorization.IAuthorizationService;

namespace Theater.Controllers
{
    [ApiController]
    [Authorize]
    public class AuthorizationController : BaseController
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
            => _authorizationService = authorizationService;

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
            var createUserResult = await _authorizationService.CreateUser(parameters);

            return RenderResult(createUserResult);
        }

        /// <summary>
        /// Возвращает пользователя по идентификатору
        /// </summary>
        /// <returns></returns>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <response code="200">В случае, если пользователь был найден в системе</response>
        /// <response code="404">В случае если пользователь не был найден</response>
        [HttpGet("user/{userId:guid}")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
        {
            var user = await _authorizationService.GetUserById(userId);

            return user is null
                ? RenderResult(UserAccountErrors.NotFound)
                : Ok(user);
        }

        //todo: переделать под параметры (пейджинация и поиск)
        /// <summary>
        /// Возвращает первые 300 пользователей отсортированные по никнейму
        /// </summary>
        /// <returns></returns>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <response code="200">В случае, если пользователь был найден в системе</response>
        /// <response code="404">В случае если пользователь не был найден</response>
        [Authorize(Policy = nameof(RoleModel.User.Policies.UserSearch))]
        [HttpPost("users")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsersByParameters()
        {
            var users = await _authorizationService.GetUsers();

            return Ok(users);
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
            var authenticateResult = await _authorizationService.Authorize(parameters);

            return authenticateResult is null
                ? RenderResult(UserAccountErrors.NotFound)
                : Ok(authenticateResult);
        }
    }
}
