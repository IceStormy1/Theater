using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using Theater.Entities.FileStorage;
using Theater.Entities.Theater;
using Theater.Entities.Users;
using System.Linq;
using Theater.Entities;
using Theater.Entities.Rooms;
using Theater.Sql.Configurations.Users;

namespace Theater.Sql;

/// <summary>
/// Контекст базы данных театра
/// </summary>
public sealed class TheaterDbContext : DbContext
{
    /// <summary>
    /// Пользователи
    /// </summary>
    public DbSet<UserEntity> Users { get; set; }

    /// <summary>
    /// Должности работников театра
    /// </summary>
    public DbSet<WorkersPositionEntity> WorkersPositions { get; set; }

    /// <summary>
    /// Жанры пьес 
    /// </summary>
    public DbSet<PiecesGenreEntity> PiecesGenres { get; set; }

    /// <summary>
    /// Работники театра
    /// </summary>
    public DbSet<TheaterWorkerEntity> TheaterWorkers { get; set; }

    /// <summary>
    /// Пьесы театра
    /// </summary>
    public DbSet<PieceEntity> Pieces { get; set; }

    /// <summary>
    /// Рецензии пользователей
    /// </summary>
    public DbSet<UserReviewEntity> UserReviews { get; set; }

    /// <summary>
    /// Репертуары пьес
    /// </summary>
    public DbSet<PieceDateEntity> PieceDates { get; set; }

    /// <summary>
    /// Билеты
    /// </summary>
    public DbSet<PiecesTicketEntity> PiecesTickets { get; set; }

    /// <summary>
    /// Работники, которые участвуют в репертуаре
    /// </summary>
    public DbSet<PieceWorkerEntity> PieceWorkers { get; set; }

    /// <summary>
    /// Забронированные билеты
    /// </summary>
    public DbSet<BookedTicketEntity> BookedTickets { get; set; }

    /// <summary>
    /// Купленные билеты пользователя
    /// </summary>
    public DbSet<PurchasedUserTicketEntity> PurchasedUserTickets { get; set; }

    /// <summary>
    /// События билетов
    /// </summary>
    public DbSet<TicketPriceEventsEntity> TicketPriceEvents { get; set; }
        
    /// <summary>
    /// Файлы в хранилище
    /// </summary>
    public DbSet<FileStorageEntity> Files { get; set; }

    /// <inheritdoc cref="RoomEntity"/>
    public DbSet<RoomEntity> Rooms { get; set; }

    /// <inheritdoc cref="UserRoomEntity"/>
    public DbSet<UserRoomEntity> UserRooms { get; set; }

    /// <inheritdoc cref="MessageEntity"/>
    public DbSet<MessageEntity> Messages { get; set; }

    public TheaterDbContext(DbContextOptions<TheaterDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
   
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(UserReviewEntityConfiguration))!);
    }

    public override int SaveChanges()
    {
        SetDates();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        SetDates();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetDates()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e =>
                (e.Entity is IHasCreatedAt or IHasUpdatedAt) && (e.State is EntityState.Added or EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added && entityEntry.Entity is IHasCreatedAt createdEntity)
                createdEntity.CreatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Modified && entityEntry.Entity is IHasUpdatedAt modifiedEntity)
                modifiedEntity.UpdatedAt = DateTime.UtcNow;
        }
    }
}