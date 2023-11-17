using Microsoft.Extensions.DependencyInjection;
//using Theater.Clients;

namespace Theater.TestTerminal;

internal sealed class Program
{
    private static async Task Main(string[] args)
    {
        // TODO: Fix Nuget packages for Docker
        var serviceCollection = new ServiceCollection();

        //ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        //var userAccountClient = serviceProvider.GetRequiredService<IUserAccountClient>();

        //await TestUserAccountMethods(userAccountClient);

        Console.ReadKey();

    }

    //private static void ConfigureServices(IServiceCollection services)
    //{
    //    services.AddTheaterClients(baseAddress: "http://localhost:5000/");
    //}

    //private static async Task TestUserAccountMethods(IUserAccountClient userAccountClient)
    //{
    //    var user = await userAccountClient.GetUserById(Guid.Parse("e1f83d38-56a7-435b-94bd-fe891ed0f03a"));

    //    Console.WriteLine(new string('-', 20));
    //}
}