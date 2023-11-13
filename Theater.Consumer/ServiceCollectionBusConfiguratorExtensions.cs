using MassTransit;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Theater.Common.Settings;
using Theater.Consumer.Consumers;

namespace Theater.Consumer;

public static class ServiceCollectionBusConfiguratorExtensions
{
    public static void RegisterMassTransit(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
    {
        var prefetchSettings = configuration
            .GetSection(nameof(ConsumerPrefetchSettings))
            .Get<ConsumerPrefetchSettings[]>() ?? Array.Empty<ConsumerPrefetchSettings>();

        var rabbitMqConnectionString = configuration.GetConnectionString("MessageBus");

        services.AddMassTransit(mt =>
        {
            var endpointNameFormatter = new EndpointNameFormatter();
            mt.SetEndpointNameFormatter(endpointNameFormatter);
           
            RegisterConsumers(services, mt, prefetchSettings, assembly, endpointNameFormatter);

            mt.UsingRabbitMq(
                (context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                    cfg.Host(rabbitMqConnectionString);

                    cfg.UseMessageRetry(x =>
                    {
                        x.Intervals(
                            TimeSpan.FromMilliseconds(500),
                            TimeSpan.FromMilliseconds(1000),
                            TimeSpan.FromMilliseconds(2000),
                            TimeSpan.FromMilliseconds(3000),
                            TimeSpan.FromMilliseconds(5000));
                    });
                });
        });
    }

    private static void RegisterConsumers(
        IServiceCollection services,
        IRegistrationConfigurator registrationConfigurator, 
        ConsumerPrefetchSettings[] prefetchSettings,
        Assembly assembly, 
        EndpointNameFormatter endpointNameFormatter)
    {
        var genericBaseConsumer = typeof(BaseEventConsumer<,>);

        foreach (var type in assembly.GetTypes())
        {
            var serviceInterface = type.GetInterfaces()
                .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IMessageConsumer<>));

            var eventType = serviceInterface?.GenericTypeArguments[0];
            if (eventType == null) continue;

            services.Add(ServiceDescriptor.Scoped(type, type));
            var consumer = genericBaseConsumer.MakeGenericType(type, eventType);

            registrationConfigurator.AddConsumer(consumer)
                .Endpoint(cfg =>
                {
                    var handlerShortName = type.ShortDisplayName();
                    var prefetchCount = prefetchSettings
                        .FirstOrDefault(settings => string.Equals(settings.ConsumerName, handlerShortName))
                        ?.PrefetchCount;

                    if (prefetchCount != null)
                        cfg.PrefetchCount = prefetchCount;

                    cfg.Name = endpointNameFormatter.GetMessageHandlerQueueName(handlerShortName);
                });
        }
    }
}