using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.Theater;

namespace Theater.Sql.Configurations.Workers;

internal sealed class PieceWorkerEntityConfiguration : IEntityTypeConfiguration<PieceWorkerEntity>
{
    public void Configure(EntityTypeBuilder<PieceWorkerEntity> builder)
    {
        builder.HasKey(x => x.Id);
    }
}