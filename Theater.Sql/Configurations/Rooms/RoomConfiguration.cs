using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.Rooms;

namespace Theater.Sql.Configurations.Rooms;

internal sealed class RoomConfiguration : IEntityTypeConfiguration<RoomEntity>
{
    public void Configure(EntityTypeBuilder<RoomEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Users)
            .WithOne(x => x.Room)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Messages)
            .WithOne(x => x.Room)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Title)
            .HasMaxLength(255);

        builder.ToTable(name: "Rooms", schema: "chat");
    }
}