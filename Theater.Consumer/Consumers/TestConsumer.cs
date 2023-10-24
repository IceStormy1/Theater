using Microsoft.Extensions.Logging;
using Theater.Contracts.Rabbit;

namespace Theater.Consumer.Consumers;

internal sealed class TestConsumer : IMessageConsumer<TestRabbitModel>
{
    private readonly ILogger<TestConsumer> _logger;

    public TestConsumer(ILogger<TestConsumer> logger)
    {
        _logger = logger;
    }

    public Task HandleMessage(TestRabbitModel message)
    {
        _logger.LogInformation("Получено сообщение {MessageId}", message.Id);

        return Task.CompletedTask;
    }
}