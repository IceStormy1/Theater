using Autofac;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Theater.Abstractions;
using Theater.Abstractions.Jwt;
using Theater.Configuration;
using Theater.Contracts.Authorization;
using Theater.Contracts.Theater;
using Theater.Core;
using Theater.Entities.Theater;
using Theater.Policy;
using Theater.Sql;
using Theater.Validation.Authorization;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace Theater
{
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
            var jwtOptionsSection = Configuration.GetSection("JwtOptions");
            services.Configure<JwtOptions>(jwtOptionsSection);

            var jwtOptions = jwtOptionsSection.Get<JwtOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateIssuer = true,

                        ValidAudience = jwtOptions.Audience,
                        ValidateAudience = true,

                        IssuerSigningKey = jwtOptions.GetSymmetricSecurityKey(), // HS256
                        ValidateIssuerSigningKey = true,

                        ValidateLifetime = true
                    };
                });

            services.Configure<RoleModel>(Configuration.GetSection("RoleModel"));
            services.Configure<FileStorageOptions>(Configuration.GetSection("FileStorageOptions"));

            var defaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = defaultPolicy;

                options.AddRoleModelPolicies<UserPolices>(Configuration, nameof(RoleModel.User));
              //  options.AddRoleModelPolicies<MwPolicies>(Configuration, nameof(RoleModel.MedicalWorker));
            });

            services.AddAllDbContext(Configuration);

            services.AddRouting(c => c.LowercaseUrls = true);

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
                });

            services.AddAutoMapper(x => x.AddMaps(typeof(MappingProfile).Assembly));

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
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.MaxDepth = 64;
                });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            
            services.AddMemoryCache();

            AddRelationRepository(services);
            AddCrudServices(services);
            AddStubValidators(services);
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
            });

            app.UseStatusCodePages();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger(c => { c.SerializeAsV2 = true; });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v{_assemblyVersion}/swagger.json", $"{ApiName} API V{_assemblyVersion}");
                c.RoutePrefix = string.Empty;
                c.DocumentTitle = $"{ApiName} Documentation";
                c.DocExpansion(DocExpansion.None);
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<Core.Module>();
            builder.RegisterModule<Core.FileStorageModule>();
        }

        private static void AddRelationRepository(IServiceCollection services)
        {
            services.AddRelationRepository<PieceEntity, TheaterDbContext>();
            services.AddRelationRepository<PieceDateEntity, TheaterDbContext>();
            services.AddRelationRepository<PieceWorkerEntity, TheaterDbContext>();
            services.AddRelationRepository<UserReviewEntity, TheaterDbContext>();
            services.AddRelationRepository<PiecesGenreEntity, TheaterDbContext>();
        }

        private static void AddCrudServices(IServiceCollection services)
        {
            services.AddScoped(typeof(ICrudService<,>), typeof(ServiceBase<,>));
            //services.AddCrudService<PieceDateParameters, PieceDateEntity>();
        }

        private static void AddStubValidators(IServiceCollection services)
        {
            services.AddStubValidator<PieceParameters>();
            services.AddStubValidator<TheaterWorkerParameters>();
            services.AddStubValidator<PiecesTicketParameters>();
            services.AddStubValidator<UserParameters>();
        }
    }
}
