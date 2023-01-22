﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Theater.Abstractions.UserAccount;
using Theater.Abstractions.UserAccount.Models;
using Theater.Contracts.Authorization;
using Theater.Contracts.UserAccount;
using Theater.Policy;

namespace Theater.Controllers
{
    [ApiController]
    [Route("api/account")]
    [Authorize]
    public sealed class UserAccountController : BaseController<IUserAccountService>
    {
        public UserAccountController(IUserAccountService userAccountService) : base(userAccountService)
        {
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
            var createUserResult = await Service.CreateUser(parameters);

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
            var user = await Service.GetUserById(userId);

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
            var users = await Service.GetUsers();

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
            var authenticateResult = await Service.Authorize(parameters);

            return authenticateResult is null
                ? RenderResult(UserAccountErrors.NotFound)
                : Ok(authenticateResult);
        }

        /// <summary>
        /// Обновить профиль пользователя
        /// </summary>
        /// <response code="200">В случае успешного запроса</response>
        /// <response code="400">В случае ошибок валидации</response>
        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser([FromBody] UserParameters parameters)
        {
            if (!UserId.HasValue)
                return RenderResult(UserAccountErrors.Unauthorized);

            var updateResult = await Service.UpdateUser(parameters, UserId.Value);

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

            var replenishResult = await Service.ReplenishBalance(UserId.Value, parameters.ReplenishmentAmount);

            return RenderResult(replenishResult);
        }
    }
}