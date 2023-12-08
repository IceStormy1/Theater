using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Globalization;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Theater.Configuration.Extensions;
using Theater.Consumer;
using Theater.Core.Profiles;
using Theater.Sql;
using Theater.Validation.Authorization;

const string apiName = "Theater";
Version assemblyVersion = new(1, 0);

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Host.UseSerilog((ctx, _, cfg) =>
    cfg
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder())
        .Enrich.WithThreadId()
        .Enrich.WithThreadName()
        .ReadFrom.Configuration(ctx.Configuration)
.MinimumLevel.Override("System", LogEventLevel.Information)
);

builder.Services.ConfigureOptions(builder.Configuration)
    .AddTheaterAuthentication(builder.Configuration)
    .AddAllDbContext(builder.Configuration)
    .AddRouting(c => c.LowercaseUrls = true);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var problemDetailsFactory =
                context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var problemDetails = problemDetailsFactory
                .CreateValidationProblemDetails(context.HttpContext, context.ModelState, statusCode: 400);
            problemDetails.Title = "Произошла ошибка валидации!";
            var result = new BadRequestObjectResult(problemDetails);

            return result;
        };
    })
    .AddNewtonsoftJson(cfg =>
    {
        cfg.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        cfg.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        cfg.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

builder.Services.AddSwaggerForApi(assemblyVersion, apiName);

builder.Services.AddMvc(opt => { opt.EnableEndpointRouting = false; })
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<UserParametersValidator>();

        fv.ValidatorOptions.LanguageManager.Enabled = true;
        fv.ValidatorOptions.LanguageManager.Culture = new CultureInfo("ru-RU");
    })
    .AddJsonOptions(x => { x.JsonSerializerOptions.MaxDepth = 64; });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services
    .AddAutoMapper(x => x.AddMaps(typeof(AbstractProfile).Assembly))
    .AddRepositories()
    .AddServices()
    .AddFileStorage()
    .AddMemoryCache()
    .AddRedis(builder.Configuration, builder.Environment)
    .RegisterMassTransit(builder.Configuration, typeof(IMessageConsumer<>).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseCors(options =>
    {
        options.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    })
    .UseStatusCodePages()
    .UseHttpsRedirection()
    .UseRouting()
    .UseSwagger(c => { c.SerializeAsV2 = true; })
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v{assemblyVersion}/swagger.json", $"{apiName} API V{assemblyVersion}");
        c.RoutePrefix = string.Empty;
        c.DocumentTitle = $"{apiName} Documentation";
        c.DocExpansion(DocExpansion.None);
    })
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Lifetime.ApplicationStopped.Register(OnShutdown);
app.Lifetime.ApplicationStarted.Register(OnStart);

MigrationTool.Execute(app.Services);

app.Run();


void OnStart()
{
    app.Logger.LogInformation("Приложение {ApiName} запущено", apiName);
}

void OnShutdown()
{
    app.Logger.LogInformation("Приложение {ApiName} остановлено", apiName);
}