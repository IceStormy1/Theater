using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Theater.Abstractions.Caches;
using Theater.Abstractions.UserAccount;
using Theater.Contracts.Rabbit;
using Theater.Contracts.UserAccount;
using Theater.Controllers.Base;

namespace Theater.Controllers;

[SwaggerTag("Тестовые методы")]
[ApiController]
[Route("[controller]")]
public sealed class TestController : BaseController
{
    private readonly IPublishEndpoint _messageBus;
    private readonly ILogger<TestController> _logger;
    private readonly IConnectionsCache _connectionsCache;

    public TestController(
        IPublishEndpoint messageBus,
        ILogger<TestController> logger,
        IConnectionsCache connectionsCache,
        IMapper mapper,
        IUserAccountService userAccountService
        ) : base(mapper, userAccountService)
    {
        _messageBus = messageBus;
        _logger = logger;
        _connectionsCache = connectionsCache;
    }

    /// <summary>
    /// Отправляет сообщение в тестовую очередь
    /// </summary>
    [HttpPost("rabbit")]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    public async Task CreateReview()
    {
        var messageId = Guid.NewGuid();

        await _messageBus.Publish(new TestRabbitModel { Id = messageId });
        _logger.LogInformation("Отправлено сообщение с идентификатором {MessageId}", messageId);
    }

    /// <summary>
    /// Добавляет соединение для залогиненного пользователя в Redis
    /// </summary>
    [HttpPost("redis/connections/{connectionId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task AddConnection(Guid connectionId)
    {
        var innerUserId = await GetUserId();

        await _connectionsCache.SetConnection(innerUserId!.Value, connectionId.ToString());
    }

    /// <summary>
    /// Получает соединения для залогиненного пользователя в Redis
    /// </summary>
    [HttpGet("redis/connections")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task GetConnections()
    {
        var innerUserId = await GetUserId();

        await _connectionsCache.GetConnections(innerUserId!.Value);
    }
}