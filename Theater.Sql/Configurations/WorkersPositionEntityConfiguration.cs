using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.Theater;

namespace Theater.Sql.Configurations
{
    internal class WorkersPositionEntityConfiguration : IEntityTypeConfiguration<WorkersPositionEntity>
    {
        public void Configure(EntityTypeBuilder<WorkersPositionEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PositionName).IsRequired().HasMaxLength(128);

            builder.HasMany(s => s.TheaterWorker)
                .WithOne(x => x.Position)
                .HasForeignKey(x => x.PositionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}