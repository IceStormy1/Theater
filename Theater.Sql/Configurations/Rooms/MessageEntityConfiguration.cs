using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.Rooms;

namespace Theater.Sql.Configurations.Rooms;

public sealed class MessageEntityConfiguration : IEntityTypeConfiguration<MessageEntity>
{
    public void Configure(EntityTypeBuilder<MessageEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Navigation(x => x.User).IsRequired();
        builder.Navigation(x => x.Room).IsRequired();

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(2000);

        builder.ToTable(name: "Messages", schema: "chat");
    }
}