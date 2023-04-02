﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Contracts;
using Theater.Entities;

namespace Theater.Controllers.Admin
{
#if DEBUG
    [Authorize]
#endif
    [Route("api/admin")]
    public class BaseAdminController<TService, TParameters, TEntity> : BaseController<TService> 
        where TService : ICrudService<TParameters, TEntity>
        where TParameters : class
        where TEntity : class, IEntity
    {
        public BaseAdminController(TService service) : base(service)
        {
        }

        /// <summary>
        /// Создать сущность
        /// </summary>
        /// <response code="200">В случае успешного запроса</response>
        /// <response code="400">В случае ошибок валидации</response>
       // [Authorize(Policy = nameof(RoleModel.User.Policies.UserSearch))]
        [HttpPost]
        [ProducesResponseType(typeof(DocumentMeta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] TParameters parameters)
            => await CreateOrUpdate(parameters);

        /// <summary>
        /// Обновить сущность
        /// </summary>
        /// <response code="200">В случае успешного запроса</response>
        /// <response code="400">В случае ошибок валидации</response>
        // [Authorize(Policy = nameof(RoleModel.User.Policies.UserSearch))]
        [HttpPut("{entityId:guid}")]
        [ProducesResponseType(typeof(DocumentMeta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] TParameters parameters, [FromRoute] Guid entityId)
            => await CreateOrUpdate(parameters, entityId);

        /// <summary>
        /// Удалить сущность
        /// </summary>
        /// <response code="200">В случае успешного запроса</response>
        /// <response code="400">В случае ошибок валидации</response>
        // [Authorize(Policy = nameof(RoleModel.User.Policies.UserSearch))]
        [HttpDelete("{entityId:guid}")]
        [ProducesResponseType(typeof(DocumentMeta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid entityId)
        {
            var deletePieceResult = await Service.Delete(entityId);

            return RenderResult(deletePieceResult);
        }

        /// <summary>
        /// Обновить или создать сущность
        /// </summary>
        /// <param name="parameters">Параметры</param>
        /// <param name="entityId">Идентификатор сущности</param>
        /// <remarks>
        /// Идентификатор <paramref name="entityId"/> указывается при обновлении сущности
        /// </remarks>
        private async Task<IActionResult> CreateOrUpdate(TParameters parameters, Guid? entityId = null)
        {
            var piecesShortInformation = await Service.CreateOrUpdate(parameters, entityId);

            return RenderResult(piecesShortInformation);
        }
    }
}
