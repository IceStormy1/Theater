using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Theater.Contracts.Authorization;
using Theater.Contracts.Messages;
using Theater.SignalR.Hubs;

namespace Theater.SignalR.TerminalChat;

public class Program
{
    private const string HubsUrl = "http://localhost:5002";
    private const string TheaterUrl = "http://localhost:5000";
    private const string LoginRoute = "/api/account/login";

    private static readonly HttpClient TheaterClient = new() { BaseAddress = new Uri(TheaterUrl) };
    private static readonly HttpClient SignalRClient = new() { BaseAddress = new Uri(HubsUrl) };

    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;
        Console.InputEncoding = Encoding.Unicode;

        var token = await GetToken();

        var roomId = args[0];

        await using var hub = new HubConnectionBuilder()
            .WithUrl(
                url: HubsUrl + ChatHub.Url,
                configureHttpConnection: options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token)!;

                    options.SkipNegotiation = true;
                    options.Transports = HttpTransportType.WebSockets;
                    options.Headers = new Dictionary<string, string>
                    {
                        { "Authorization", $"Bearer {token}" }
                    };
                    options.WebSocketFactory = async (context, cancellationToken) =>
                    {
                        var webSocketClient = new ClientWebSocket();
                        webSocketClient.Options.SetRequestHeader("Authorization", $"Bearer {token}");
                        var url = new Uri($"{context.Uri}?access_token={token}");
                        await webSocketClient.ConnectAsync(url, cancellationToken);
                        return webSocketClient;
                    };
                })
            .WithAutomaticReconnect()
            .Build();

        await hub.StartAsync();

        hub.On<Guid, MessageModel>("OnMessageReceived", (roomId1, message) =>
        {
            Console.WriteLine(new StringBuilder("Reply - ").Append(Environment.NewLine)
                .Append($"Id: {message.Id}, ").Append(Environment.NewLine)
                .Append($"Date: {message.CreatedAt}, ").Append(Environment.NewLine)
                .Append($"RoomId: {roomId1}, ").Append(Environment.NewLine)
                .Append($"Author: {message.AuthorId},  ").Append(Environment.NewLine)
                .Append($"Message: {message.Text}").Append(Environment.NewLine)
                .ToString());
        });

        //hub.On<ReadMessageDto>("OnMessageRead", (message) =>
        //{
        //    Console.WriteLine(new StringBuilder("Пользователь прочитал сообщение - ")
        //        .Append($"Id: {message.MessageId}, ")
        //        .ToString());
        //});


        //hub.On<long, RoomType, string>("OnRoomEnter", (roomId, roomType, title) =>
        //{
        //    Console.WriteLine(new StringBuilder("Пользователь вошел в чат - ")
        //        .Append($"RoomId: {roomId}, ").Append(Environment.NewLine)
        //        .Append($"Title: {title}, ").Append(Environment.NewLine)
        //        .ToString());
        //});

        //hub.On<Guid>("OnRoomExit", (id) =>
        //{
        //    Console.WriteLine(new StringBuilder("Пользователь покинул чат - ")
        //        .Append($"RoomId: {id}, ").Append(Environment.NewLine)
        //        .ToString());
        //});

        hub.On<List<Guid>>("UpdateUsersAsync", (List<Guid> users) =>
        {
            Console.WriteLine($"Total Users: {users.Count}");

            foreach (var user in users)
            {
                Console.WriteLine($"User Id: {user}");
            }
        });

        hub.On<Guid, string>("SendMessageAsync", (id, message) =>
        {
            Console.WriteLine($"Пользователь {id} отправил сообщение: {message}");
        });

        var tokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += (sender, eventArgs) => { tokenSource.Cancel(); };

        TheaterClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        Console.WriteLine("Чат запущен. Пиши сообщение и жми enter");

        while (!tokenSource.IsCancellationRequested)
        {
            var message = Console.ReadLine();
            var response = await TheaterClient.PostAsJsonAsync(
                requestUri: $"api/rooms/{roomId}/message",
                value: new NewMessageModel { Text = message }, 
                cancellationToken: tokenSource.Token);

            response.EnsureSuccessStatusCode();

            await Task.Delay(100, tokenSource.Token);
        }


        Console.ReadLine();
        await hub.StopAsync(tokenSource.Token);
    }

    private static async Task<string> GetToken()
    {
        var authParameters = new { UserName = "IceStormy-admin", Password = "123456" };
        const string requestUri = TheaterUrl + LoginRoute;
        var loginResponse = await TheaterClient.PostAsJsonAsync(requestUri, value: authParameters);

        loginResponse.EnsureSuccessStatusCode();

        var authResponse = await loginResponse.Content.DeserializeContent<AuthenticateResponse>();

        return authResponse.AccessToken;
    }
}