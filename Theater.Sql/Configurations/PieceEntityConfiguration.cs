using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.Theater;

namespace Theater.Sql.Configurations
{
    internal sealed class PieceEntityConfiguration : IEntityTypeConfiguration<PieceEntity>
    {
        public void Configure(EntityTypeBuilder<PieceEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PieceName).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(512);

            builder.HasMany(s => s.UserReviews)
                .WithOne(x => x.Piece)
                .HasForeignKey(x => x.PieceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.PieceDates)
                .WithOne(x => x.Piece)
                .HasForeignKey(x => x.PieceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.PieceWorkers)
                .WithOne(x => x.Piece)
                .HasForeignKey(x => x.PieceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}