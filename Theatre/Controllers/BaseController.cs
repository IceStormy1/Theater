using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using Theater.Common;

namespace Theater.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<TService> : ControllerBase
    {
        protected readonly TService Service;

        protected Guid? UserId => Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public BaseController(TService service)
        {
            Service = service;
        }

        /// <summary>
        /// Returns ActionResult from the given WriteResult
        /// </summary>
        /// <remarks>
        /// Use generic param to define in which model successful result should be mapped
        /// </remarks>
        /// <param name="source">Write result with error or data</param>
        /// <typeparam name="T">Desired response model</typeparam>
        protected IActionResult RenderResult<T>(IWriteResult<T> source)
        {
            if (!source.IsSuccess)
                return RenderError(source.Error);

            if (source.ResultData == null)
                return new NoContentResult();

            return new JsonResult(source.ResultData);
        }

        /// <summary>
        /// Returns ActionResult from the given WriteResult
        /// </summary>
        /// <param name="source">Write result</param>
        protected IActionResult RenderResult(WriteResult source)
            => source.IsSuccess ? new OkResult() : RenderError(source.Error);

        /// <summary>
        /// Returns action result for error as <see cref="ProblemDetails"/> with proper status code
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
    }
}
