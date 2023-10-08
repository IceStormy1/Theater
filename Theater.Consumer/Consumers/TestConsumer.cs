using MassTransit;
using Microsoft.Extensions.Logging;
using Theater.Contracts.Rabbit;

namespace Theater.Consumer.Consumers;

internal sealed class TestConsumer : IConsumer<TestRabbitModel>
{
    private readonly ILogger<TestConsumer> _logger;

    public TestConsumer(ILogger<TestConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TestRabbitModel> context)
    {
        _logger.LogInformation("Получено сообщение {MessageId}", context.Message.Id);

        return Task.CompletedTask;
    }
}