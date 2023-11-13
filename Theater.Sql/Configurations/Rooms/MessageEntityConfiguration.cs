using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Common.Constants;
using Theater.Entities.Rooms;

namespace Theater.Sql.Configurations.Rooms;

public sealed class MessageEntityConfiguration : IEntityTypeConfiguration<MessageEntity>
{
    public void Configure(EntityTypeBuilder<MessageEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(MessageConstants.MessageMaxLength);

        builder.ToTable(name: "Messages", schema: "chat");
    }
}