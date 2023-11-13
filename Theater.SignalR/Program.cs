using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using Theater.Configuration.Extensions;
using Theater.Consumer;
using Theater.Core.Profiles;
using Theater.SignalR.Hubs;
using Theater.Sql;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

const string apiName = "Theater.SignalR";
const string corsPolicy = "CorsPolicy";
const string selfCheck = "self";

Version assemblyVersion = new(1, 0);

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddSignalR();

builder.Host.UseSerilog((ctx, _, cfg) =>
    cfg
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder())
        .Enrich.WithThreadId()
        .Enrich.WithThreadName()
        .ReadFrom.Configuration(ctx.Configuration)
        .MinimumLevel.Override("System", LogEventLevel.Information)
);

builder.Services.AddTheaterAuthentication(builder.Configuration)
    .AddAllDbContext(builder.Configuration);
builder.Services.AddControllers();


builder.Services
    .AddEndpointsApiExplorer()
    .AddRouting(c => c.LowercaseUrls = true)
    .AddSwaggerGen(c =>
    {
        c.CustomSchemaIds(type => type.ToString());
        c.CustomOperationIds(d => (d.ActionDescriptor as ControllerActionDescriptor)?.ActionName);
        c.SwaggerDoc($"v{assemblyVersion}", new OpenApiInfo
        {
            Version = $"v{assemblyVersion}",
            Title = $"{apiName} API",
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        var xmlContractDocs = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory), "*.xml");
        foreach (var fileName in xmlContractDocs) c.IncludeXmlComments(fileName);

        c.EnableAnnotations();
        c.AddEnumsWithValuesFixFilters();
        c.AddSignalRSwaggerGen();
    })
    .AddCors(options =>
    {
        options.AddPolicy(corsPolicy, policyBuilder =>
            policyBuilder
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod());
    })
    .AddAutoMapper(x => x.AddMaps(typeof(AbstractProfile).Assembly))
    .AddRepositories()
    .AddServices().AddSingleton<ChatManager>()
    .AddFileStorage()
    .AddMemoryCache()
    .RegisterMassTransit(builder.Configuration, Assembly.GetExecutingAssembly())
    ;

builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

builder.Services.AddHealthChecks()
    .AddCheck(selfCheck, () => HealthCheckResult.Healthy());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c => { c.SerializeAsV2 = true; });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v{assemblyVersion}/swagger.json", $"{apiName} API V{assemblyVersion}");
        c.RoutePrefix = string.Empty;
        c.DocumentTitle = $"{apiName} Documentation";
        c.DocExpansion(DocExpansion.None);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStopped.Register(OnShutdown);
app.Lifetime.ApplicationStarted.Register(OnStart);

app.UseHttpsRedirection()
    .Use((context, next) =>
    {
        // Необходимо для того, чтобы передать токен доступа в хедер.
        // Бразуеры на данный момент развития технологий не умеют заполнять хэдер
        // при сокетном подключении в signalR.
        var accessToken = context.Request.Query["access_token"];

        var path = context.Request.Path;
        if (!string.IsNullOrEmpty(accessToken) &&
            (path.StartsWithSegments(ChatHub.Url)))
        {
            context.Request.Headers.Authorization = "Bearer " + accessToken;
        }

        return next();
    })
    .UseAuthentication()
    .UseRouting()
    .UseCors(corsPolicy) 
    .UseAuthorization();

app.MapHub<ChatHub>(ChatHub.Url);
app.MapHealthChecks("/liveness", new HealthCheckOptions { Predicate = r => r.Name == selfCheck });
app.MapHealthChecks("/health");

MigrationTool.Execute(app.Services);

app.Run();


void OnStart()
{
    app.Logger.LogInformation($"Приложение {apiName} запущено");
}

void OnShutdown()
{
    app.Logger.LogInformation($"Приложение {apiName} остановлено");
}