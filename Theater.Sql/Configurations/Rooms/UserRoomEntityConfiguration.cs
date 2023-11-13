using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.Rooms;

namespace Theater.Sql.Configurations.Rooms;

internal sealed class UserRoomEntityConfiguration : IEntityTypeConfiguration<UserRoomEntity>
{
    public void Configure(EntityTypeBuilder<UserRoomEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.UserId, x.RoomId }).IsUnique();

        builder.Property(x => x.IsActive).HasDefaultValue(true);
        
        builder.HasOne(x => x.LastReadMessage)
            .WithMany(x => x.UserRooms)
            .HasForeignKey(x => x.LastReadMessageId);

        builder.ToTable(name: "UserRooms", schema: "chat");
    }
}