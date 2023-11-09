using Microsoft.Extensions.DependencyInjection;
using Polly;
using Refit;
using System.Net.Sockets;

namespace Theater.Clients;

public static class TheaterExtensions
{
    private const string HttpClientName = "Theater.Clients";

    /// <summary>
    /// Регистрирует все клиенты в контейнере зависимостей.
    /// </summary>
    /// <param name="services">Контейнер зависимостей.</param>
    /// <param name="baseAddress">Базовый адрес для вызовов.</param>
    /// <param name="clientConfigure"></param>
    private static void AddTheaterClients(this IServiceCollection services, Uri baseAddress, Action<IHttpClientBuilder>? clientConfigure = null)
    {
        var retryTimeouts = Enumerable
            .Range(1, 3)
            .Select(x => TimeSpan.FromMilliseconds(x * 500))
            .ToArray();

        var clientBuilder = services.AddHttpClient(HttpClientName, config => { config.BaseAddress = baseAddress; })
            .AddTransientHttpErrorPolicy(p => p.Or<SocketException>().WaitAndRetryAsync(retryTimeouts));

        clientConfigure?.Invoke(clientBuilder);

        services.AddRestClient<IUserAccountClient>();
    }

    /// <summary>
    /// Регистрирует все клиенты в контейнере зависимостей.
    /// </summary>
    /// <param name="services">Контейнер зависимостей.</param>
    /// <param name="baseAddress">Базовый адрес для вызовов.</param>
    /// <param name="clientConfigure"></param>
    public static void AddTheaterClients(this IServiceCollection services, string baseAddress, Action<IHttpClientBuilder>? clientConfigure = null) =>
        services.AddTheaterClients(new Uri(baseAddress), clientConfigure);

    private static IServiceCollection AddRestClient<T>(this IServiceCollection services) where T : class
    {
        services.AddSingleton(_ => RequestBuilder.ForType<T>());
        services.AddTransient(p =>
        {
            var httpClient = p.GetRequiredService<IHttpClientFactory>().CreateClient(HttpClientName);

            var refitSettings = new RefitSettings(new NewtonsoftJsonContentSerializer());
            var requestBuilder = RequestBuilder.ForType<T>(refitSettings);

            return RestService.For(httpClient, requestBuilder);
        });

        return services;
    }
}