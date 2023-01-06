using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Theater.Entities.Authorization;
using Theater.Entities.FileStorage;
using Theater.Entities.Theater;
using Theater.Sql.Configurations;

namespace Theater.Sql
{
    /// <summary>
    /// Контекст базы данных театра
    /// </summary>
    public class TheaterDbContext : DbContext
    {
        /// <summary>
        /// Пользователи
        /// </summary>
        public DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// Роли пользователей
        /// </summary>
        public DbSet<UserRoleEntity> UserRoles { get; set; }

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
        public DbSet<BookedTicketEntity> BookedTicketsEntity { get; set; }

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

        public TheaterDbContext(DbContextOptions<TheaterDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(UserReviewEntityConfiguration))!);
        }
    }
}
