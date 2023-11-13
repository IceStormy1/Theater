using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.Theater;

namespace Theater.Sql.Configurations.Pieces;

internal sealed class PiecesGenreEntityConfiguration : IEntityTypeConfiguration<PiecesGenreEntity>
{
    public void Configure(EntityTypeBuilder<PiecesGenreEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.GenreName).IsRequired().HasMaxLength(64);

        builder.HasMany(s => s.Pieces)
            .WithOne(x => x.Genre)
            .HasForeignKey(x => x.GenreId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}