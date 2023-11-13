using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Theater.Common.Settings;
using Theater.Configuration.Extensions;
using Theater.Consumer;
using Theater.Core.Profiles;
using Theater.Sql;
using Theater.Validation.Authorization;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace Theater;

public sealed class Startup
{
    private const string ApiName = "Theater";
    private readonly Version _assemblyVersion = new(1, 0);

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<FileStorageOptions>(Configuration.GetSection(nameof(FileStorageOptions)));

        services.AddTheaterAuthentication(Configuration)
            .AddAllDbContext(Configuration)
            .AddRouting(c => c.LowercaseUrls = true);
     
        services.AddControllers()
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

        services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(type => type.ToString());
            c.CustomOperationIds(d => (d.ActionDescriptor as ControllerActionDescriptor)?.ActionName);
            c.SwaggerDoc($"v{_assemblyVersion}", new OpenApiInfo
            {
                Version = $"v{_assemblyVersion}",
                Title = $"{ApiName} API",
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            var xmlContractDocs = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory), "*.xml");
            foreach (var fileName in xmlContractDocs) c.IncludeXmlComments(fileName);
           
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });

            c.EnableAnnotations();
            c.AddEnumsWithValuesFixFilters();
        });

        services.AddMvc(opt => { opt.EnableEndpointRouting = false; })
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<UserParametersValidator>();

                fv.ValidatorOptions.LanguageManager.Enabled = true;
                fv.ValidatorOptions.LanguageManager.Culture = new CultureInfo("ru-RU");
            })
            .AddJsonOptions(x => { x.JsonSerializerOptions.MaxDepth = 64; });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddAutoMapper(x => x.AddMaps(typeof(AbstractProfile).Assembly))
            .AddRepositories()
            .AddServices()
            .AddFileStorage()
            .AddMemoryCache()
            .RegisterMassTransit(Configuration, typeof(IMessageConsumer<>).Assembly);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
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
                c.SwaggerEndpoint($"/swagger/v{_assemblyVersion}/swagger.json", $"{ApiName} API V{_assemblyVersion}");
                c.RoutePrefix = string.Empty;
                c.DocumentTitle = $"{ApiName} Documentation";
                c.DocExpansion(DocExpansion.None);
            })
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}