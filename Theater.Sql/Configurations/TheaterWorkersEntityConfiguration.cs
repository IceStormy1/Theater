using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.Theater;

namespace Theater.Sql.Configurations
{
    internal sealed class TheaterWorkersEntityConfiguration : IEntityTypeConfiguration<TheaterWorkerEntity>
    {
        public void Configure(EntityTypeBuilder<TheaterWorkerEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(128);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(128);
            builder.Property(x => x.MiddleName).HasMaxLength(128);
            builder.Property(x => x.Description).HasMaxLength(512);

            builder.HasMany(s => s.PieceWorkers)
                .WithOne(x => x.TheaterWorker)
                .HasForeignKey(x => x.TheaterWorkerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
