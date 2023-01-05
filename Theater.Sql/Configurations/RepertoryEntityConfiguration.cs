using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.Theater;

namespace Theater.Sql.Configurations
{
    internal class RepertoryEntityConfiguration : IEntityTypeConfiguration<RepertoryEntity>
    {
        public void Configure(EntityTypeBuilder<RepertoryEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Time).IsRequired();

            builder.HasMany(s => s.PiecesTickets)
                .WithOne(x => x.Repertory)
                .HasForeignKey(x => x.RepertoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.RepertoriesWorkers)
                .WithOne(x => x.Repertory)
                .HasForeignKey(x => x.RepertoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
