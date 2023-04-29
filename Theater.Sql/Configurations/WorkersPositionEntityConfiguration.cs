using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using Theater.Common;
using Theater.Entities.Theater;

namespace Theater.Sql.Configurations;

internal sealed class WorkersPositionEntityConfiguration : IEntityTypeConfiguration<WorkersPositionEntity>
{
    public void Configure(EntityTypeBuilder<WorkersPositionEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.PositionName).IsRequired().HasMaxLength(128);

        builder.HasMany(s => s.TheaterWorker)
            .WithOne(x => x.Position)
            .HasForeignKey(x => x.PositionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData(GetWorkersPositions());
    }

    private static IEnumerable<WorkersPositionEntity> GetWorkersPositions()
        => new List<WorkersPositionEntity>
        {
            new ()
            {
                Id = Guid.NewGuid(),
                PositionName = "Заслуженный актер",
                PositionType = PositionType.Actor
            },
            new()
            {
                Id = Guid.NewGuid(),
                PositionName = "Художник",
                PositionType = PositionType.Artist
            },
            new()
            {
                Id = Guid.NewGuid(),
                PositionName = "Гитарист",
                PositionType = PositionType.Musician
            },
            new()
            {
                Id = Guid.NewGuid(),
                PositionName = "Режиссер-постановщик",
                PositionType = PositionType.Producer
            }
        };
}