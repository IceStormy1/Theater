using Amazon.S3;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using Theater.Abstractions;
using Theater.Abstractions.Authorization;
using Theater.Abstractions.Filters;
using Theater.Abstractions.Jwt;
using Theater.Abstractions.PieceDates;
using Theater.Abstractions.PurchasedUserTicket;
using Theater.Abstractions.TheaterWorker;
using Theater.Abstractions.UserAccount;
using Theater.Abstractions.UserReviews;
using Theater.Common.Settings;
using Theater.Configuration.Policy;
using Theater.Contracts.Rooms;
using Theater.Contracts.Theater.Piece;
using Theater.Contracts.Theater.PieceDate;
using Theater.Contracts.Theater.PiecesGenre;
using Theater.Contracts.Theater.PiecesTicket;
using Theater.Contracts.Theater.PieceWorker;
using Theater.Contracts.Theater.PurchasedUserTicket;
using Theater.Contracts.Theater.TheaterWorker;
using Theater.Contracts.Theater.UserReview;
using Theater.Contracts.Theater.WorkersPosition;
using Theater.Contracts.UserAccount;
using Theater.Core;
using Theater.Core.Authorization;
using Theater.Core.Theater.Validators;
using Theater.Entities;
using Theater.Entities.Rooms;
using Theater.Entities.Theater;
using Theater.Entities.Users;
using Theater.Sql;
using Theater.Sql.QueryBuilders;
using Theater.Sql.Repositories;
using VkNet;
using VkNet.Abstractions;
using PieceIndexReader = Theater.Sql.IndexReader<Theater.Contracts.Theater.Piece.PieceModel, Theater.Entities.Theater.PieceEntity, Theater.Abstractions.Filters.PieceFilterSettings>;
using TheaterWorkerIndexReader = Theater.Sql.IndexReader<Theater.Contracts.Theater.TheaterWorker.TheaterWorkerModel, Theater.Entities.Theater.TheaterWorkerEntity, Theater.Abstractions.Filters.TheaterWorkerFilterSettings>;
using TicketIndexReader = Theater.Sql.IndexReader<Theater.Contracts.Theater.PurchasedUserTicket.PurchasedUserTicketModel, Theater.Entities.Theater.PurchasedUserTicketEntity, Theater.Abstractions.Filters.PieceTicketFilterSettings>;
using UserIndexReader = Theater.Sql.IndexReader<Theater.Contracts.UserAccount.UserModel, Theater.Entities.Users.UserEntity, Theater.Abstractions.Filters.UserAccountFilterSettings>;
using UserReviewIndexReader = Theater.Sql.IndexReader<Theater.Contracts.Theater.UserReview.UserReviewModel, Theater.Entities.Theater.UserReviewEntity, Theater.Abstractions.Filters.UserReviewFilterSettings>;

namespace Theater.Configuration.Extensions;

public static class ServiceCollectionExtensions
{
    private const string ServiceSuffix = "Service";
    private const string RepositorySuffix = "Repository";

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        var serviceTypes = typeof(BaseCrudService<,>).Assembly
            .GetTypes()
            .Where(x => x.Name.EndsWith(ServiceSuffix) && !x.IsAbstract && !x.IsInterface)
            .ToList();

        //services.AddCrudServices();
        services.RegisterImplementations(serviceTypes)
            .AddStubValidators()
            .AddScoped<IJwtHelper, JwtHelper>()
            .AddScoped<IVkApi>(_ => new VkApi())
            .AddValidators()
            .AddIndexReaders()
            ;

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        var repositories = typeof(BaseCrudRepository<>).Assembly
            .GetTypes()
            .Where(x => x.Name.EndsWith(RepositorySuffix) && !x.IsAbstract && !x.IsInterface)
            .ToList();

        return services.RegisterImplementations(repositories)
            .AddRelationRepositories();
    }

    public static IServiceCollection AddTheaterAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptionsSection = configuration.GetSection("JwtOptions");
        services.Configure<JwtOptions>(jwtOptionsSection);
        var jwtOptions = jwtOptionsSection.Get<JwtOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                if (jwtOptions != null)
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

        services.Configure<RoleModel>(configuration.GetSection("RoleModel"));
        services.Configure<FileStorageOptions>(configuration.GetSection("FileStorageOptions"));

        var defaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = defaultPolicy;

            options.AddRoleModelPolicies<UserPolices>(configuration, nameof(RoleModel.User));
            //  options.AddRoleModelPolicies<MwPolicies>(Configuration, nameof(RoleModel.MedicalWorker));
        });

        return services;
    }

    public static IServiceCollection AddFileStorage(this IServiceCollection services)
    {
        services.AddSingleton<IAmazonS3>(p =>
        {
            var options = p.GetRequiredService<IOptions<FileStorageOptions>>();
            var config = new AmazonS3Config
            {
                AuthenticationRegion = options.Value.Region, // Should match the `MINIO_REGION` environment variable.
                ServiceURL = options.Value.ServiceInnerUrl,
                ForcePathStyle = true // MUST be true to work correctly with MinIO server
            };

            return new AmazonS3Client(options.Value.AccessKey, options.Value.SecretKey, config);
        });

        return services;
    }

    private static IServiceCollection RegisterImplementations(this IServiceCollection services, IEnumerable<Type> implementationTypes)
    {
        try
        {
            foreach (var implementationType in implementationTypes)
            {
                var serviceInterface = implementationType.GetInterfaces();

                if (serviceInterface.Length > 0)
                {
                    foreach (var @interface in serviceInterface)
                    {
                        services.AddScoped(@interface, implementationType);
                    }
                }
                else
                {
                    services.AddScoped(implementationType);
                }
            }
        }
        catch (Exception)
        {
            // ignored
        }

        return services;
    }

    private static IServiceCollection AddRelationRepository<TEntity, TDbContext>(this IServiceCollection services)
        where TEntity : class, IEntity
        where TDbContext : TheaterDbContext
    {
        return services.AddScoped<ICrudRepository<TEntity>>(p =>
            new BaseCrudRepository<TEntity>(p.GetRequiredService<TDbContext>(), p.GetRequiredService<ILogger<BaseCrudRepository<TEntity>>>()));
    }

    private static IServiceCollection AddStubValidator<T>(this IServiceCollection services) where T : class
        => services.AddSingleton<IDocumentValidator<T>, DocumentValidatorStub<T>>();

    private static IServiceCollection AddRelationRepositories(this IServiceCollection services)
    {
        services.AddRelationRepository<PieceEntity, TheaterDbContext>();
        services.AddRelationRepository<PieceDateEntity, TheaterDbContext>();
        services.AddRelationRepository<PieceWorkerEntity, TheaterDbContext>();
        services.AddRelationRepository<UserReviewEntity, TheaterDbContext>();
        services.AddRelationRepository<PiecesGenreEntity, TheaterDbContext>();
        services.AddRelationRepository<WorkersPositionEntity, TheaterDbContext>();
        services.AddRelationRepository<PurchasedUserTicketEntity, TheaterDbContext>();
        services.AddRelationRepository<MessageEntity, TheaterDbContext>();
        services.AddRelationRepository<RoomEntity, TheaterDbContext>();

        return services;
    }

    private static IServiceCollection AddStubValidators(this IServiceCollection services)
    {
        services.AddStubValidator<PieceParameters>()
        .AddStubValidator<TheaterWorkerParameters>()
        .AddStubValidator<PiecesTicketParameters>()
        .AddStubValidator<UserParameters>()
        .AddStubValidator<RoomParameters>();

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IDocumentValidator<PieceDateParameters>, PiecesDateValidator>()
            .AddScoped<IDocumentValidator<PieceWorkerParameters>, PieceWorkersValidator>()
            .AddScoped<IDocumentValidator<PiecesGenreParameters>, PieceGenreValidator>()
            .AddScoped<IDocumentValidator<WorkersPositionParameters>, WorkersPositionValidator>()
            .AddScoped<IDocumentValidator<UserReviewParameters>, UserReviewValidator>();

        return services;
    }

    private static IServiceCollection AddIndexReaders(this IServiceCollection services)
    {
        services.Scan(x => x.FromAssemblies(typeof(QueryBuilderBase<,>).Assembly)
                .AddClasses(c => c.Where(v => v.Name.EndsWith("Builder")))
                .AsSelf()
                .AsImplementedInterfaces()
                .WithScopedLifetime())
            .AddScoped<IIndexReader<PieceModel, PieceEntity, PieceFilterSettings>, PieceIndexReader>(p =>
                new PieceIndexReader(
                    p.GetRequiredService<TheaterDbContext>(),
                    p.GetRequiredService<PieceQueryBuilder>(),
                    p.GetRequiredService<IPieceRepository>(),
                    p.GetRequiredService<IMapper>()))
            .AddScoped<IIndexReader<TheaterWorkerModel, TheaterWorkerEntity, TheaterWorkerFilterSettings>, TheaterWorkerIndexReader>(p =>
                new TheaterWorkerIndexReader(
                    p.GetRequiredService<TheaterDbContext>(),
                    p.GetRequiredService<TheaterWorkerQueryBuilder>(),
                    p.GetRequiredService<ITheaterWorkerRepository>(),
                    p.GetRequiredService<IMapper>()
                ))
            .AddScoped<IIndexReader<UserModel, UserEntity, UserAccountFilterSettings>, UserIndexReader>(p =>
                new UserIndexReader(
                    p.GetRequiredService<TheaterDbContext>(),
                    p.GetRequiredService<UserQueryBuilder>(),
                    p.GetRequiredService<IUserAccountRepository>(),
                    p.GetRequiredService<IMapper>()
                ))
            .AddScoped<IIndexReader<UserReviewModel, UserReviewEntity, UserReviewFilterSettings>, UserReviewIndexReader>(p =>
                new UserReviewIndexReader(
                    p.GetRequiredService<TheaterDbContext>(),
                    p.GetRequiredService<UserReviewQueryBuilder>(),
                    p.GetRequiredService<IUserReviewsRepository>(),
                    p.GetRequiredService<IMapper>()
                ))
            .AddScoped<IIndexReader<PurchasedUserTicketModel, PurchasedUserTicketEntity, PieceTicketFilterSettings>, TicketIndexReader>(p =>
                new TicketIndexReader(
                    p.GetRequiredService<TheaterDbContext>(),
                    p.GetRequiredService<PurchasedUserTicketQueryBuilder>(),
                    p.GetRequiredService<IPurchasedUserTicketRepository>(),
                    p.GetRequiredService<IMapper>()
                ));

        return services;
    }
}