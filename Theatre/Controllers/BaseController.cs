using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using Theater.Abstractions;
using Theater.Abstractions.Authorization.Models;
using Theater.Common;
using Theater.Entities;

namespace Theater.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<TParameters, TEntity> : ControllerBase
        where TParameters : class
        where TEntity : class, IEntity
    {
        protected readonly ICrudService<TParameters, TEntity> Service;

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        protected Guid? UserId => Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        /// <summary>
        /// Роль пользователя
        /// </summary>
        protected UserRole? UserRole => GetUserRoleFromToken();

        public BaseController(ICrudService<TParameters, TEntity> service)
        {
            Service = service;
        }

        /// <summary>
        /// Возвращает ActionResult из WriteResult
        /// </summary>
        /// <param name="source">Write result с ошибкой или моделью</param>
        /// <typeparam name="T"></typeparam>
        protected IActionResult RenderResult<T>(IWriteResult<T> source)
        {
            if (!source.IsSuccess)
                return RenderError(source.Error);

            if (source.ResultData == null)
                return new NoContentResult();

            return new JsonResult(source.ResultData);
        }

        /// <summary>
        /// Возвращает ActionResult из WriteResult
        /// </summary>
        /// <param name="source">Write result</param>
        protected IActionResult RenderResult(WriteResult source)
            => source.IsSuccess ? new OkResult() : RenderError(source.Error);

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
        /// <returns></returns>
        private UserRole? GetUserRoleFromToken()
            => Enum.TryParse<UserRole>(User.Claims.First(x => x.Type == ClaimTypes.Role).Value, true, out var userRole)
                ? userRole
                : null;
    }
}
