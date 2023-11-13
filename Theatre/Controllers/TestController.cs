using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using Theater.Contracts.Rabbit;
using Theater.Contracts.UserAccount;

namespace Theater.Controllers;

[SwaggerTag("Тестовые методы")]
[ApiController]
[Route("[controller]")]
public sealed class TestController : ControllerBase
{
    private readonly IPublishEndpoint _messageBus;
    private readonly ILogger<TestController> _logger;

    public TestController(IPublishEndpoint messageBus, ILogger<TestController> logger)
    {
        _messageBus = messageBus;
        _logger = logger;
    }

    /// <summary>
    /// Отправляет сообщение в тестовую очередь
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    public async Task CreateReview()
    {
        var messageId = Guid.NewGuid();

        await _messageBus.Publish(new TestRabbitModel { Id = messageId });
        _logger.LogInformation("Отправлено сообщение с идентификатором {MessageId}", messageId);
    }
}