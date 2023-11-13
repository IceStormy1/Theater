namespace Theater.Consumer;

public interface IMessageConsumer<in T> where T : class
{
    Task ProcessMessage(T message);
}