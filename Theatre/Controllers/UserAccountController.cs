using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Theater.Abstractions.UserAccount;
using Theater.Abstractions.UserAccount.Models;
using Theater.Contracts.UserAccount;

namespace Theater.Controllers
{
    [ApiController]
    [Route("api/account")]
    [Authorize]
    public class UserAccountController : BaseController<IUserAccountService>
    {
        public UserAccountController(IUserAccountService userAccountService) : base(userAccountService)
        {
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