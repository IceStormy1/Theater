using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Common.Enums;
using Theater.Entities.Theater;

namespace Theater.Sql.Configurations.Workers;

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
                Id = Guid.Parse("f5b16125-f017-46a6-a6e3-feaa8ca43d68"),
                PositionName = "Заслуженный актер",
                PositionType = PositionType.Actor
            },
            new()
            {
                Id = Guid.Parse("631c7430-2620-4523-b8a1-a2c0634e85bd"),
                PositionName = "Художник",
                PositionType = PositionType.Artist
            },
            new()
            {
                Id = Guid.Parse("ebf8a51a-8dc3-4a46-8c15-ced2e1d15736"),
                PositionName = "Гитарист",
                PositionType = PositionType.Musician
            },
            new()
            {
                Id = Guid.Parse("67bb936a-3bd6-4c6c-aaa2-5e3a55d5fdc2"),
                PositionName = "Режиссер-постановщик",
                PositionType = PositionType.Producer
            }
        };
}