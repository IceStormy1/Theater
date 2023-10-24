using MassTransit;

namespace Theater.Consumer.Consumers;

public class BaseEventConsumer<TConsumer, TEvent> : IConsumer<TEvent>
    where TEvent : class 
    where TConsumer : IMessageConsumer<TEvent>
{
    private readonly TConsumer _handler;

    public BaseEventConsumer(TConsumer handler)
    {
        _handler = handler;
    }

    public Task Consume(ConsumeContext<TEvent> context) =>
        context.Message == null ? Task.CompletedTask : _handler.HandleMessage(context.Message);
}