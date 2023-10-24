using MassTransit;

namespace Theater.Consumer;

internal sealed class EndpointNameFormatter : DefaultEndpointNameFormatter
{
    private const string PrefixQueue = "Theater-";

    public override string SanitizeName(string name)
        => PrefixQueue + name;

    /// <summary>
    /// Получить название очереди для обработчика сообщений
    /// </summary>
    /// <param name="messageHandlerTypeName">Название производного типа от <see cref="IMessageConsumer{T}"/></param>
    /// <returns></returns>
    public string GetMessageHandlerQueueName(string messageHandlerTypeName)
        => $"{PrefixQueue}{messageHandlerTypeName}";
}