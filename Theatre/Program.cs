using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Theater.Sql;

namespace Theater;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        MigrationTool.Execute(host.Services);
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;

                config.AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                    .AddEnvironmentVariables();
            })
            .UseSerilog((ctx, _, cfg) =>
                cfg
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder())
                    .Enrich.WithThreadId()
                    .Enrich.WithThreadName()
                    .ReadFrom.Configuration(ctx.Configuration)
                    .MinimumLevel.Override("System", LogEventLevel.Information)
            )
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}