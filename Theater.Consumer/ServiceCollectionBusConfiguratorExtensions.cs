using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Theater.Consumer.Consumers;
using Theater.Consumer.Settings;
using Theater.Contracts.Rabbit;

namespace Theater.Consumer;

public static class ServiceCollectionBusConfiguratorExtensions
{
    public static void RegisterMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var prefetchSettings = configuration
            .GetSection(nameof(ConsumerPrefetchSettings))
            .Get<ConsumerPrefetchSettings[]>() ?? Array.Empty<ConsumerPrefetchSettings>();

        var rabbitMqConnectionString = configuration.GetConnectionString("MessageBus");

        services.AddMassTransit(mt =>
        {
            var endpointNameFormatter = new EndpointNameFormatter();
            mt.SetEndpointNameFormatter(endpointNameFormatter);
           
            mt.RegisterConsumers(prefetchSettings);

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

    private static void RegisterConsumers(this IRegistrationConfigurator services, ConsumerPrefetchSettings[] settings)
    {
        services.AddConsumer<TestConsumer>().Endpoint(cfg => cfg.ConfigureEndpoint(settings, nameof(TestRabbitModel)));
    }

    private static void ConfigureEndpoint(this IEndpointRegistrationConfigurator configurator, IEnumerable<ConsumerPrefetchSettings> settings, string consumerName)
    {
        var prefetchCount = settings
            .FirstOrDefault(eventHandler => string.Equals(eventHandler.ConsumerName, consumerName))?.PrefetchCount;

        if (prefetchCount != null)
            configurator.PrefetchCount = prefetchCount;
    }
}