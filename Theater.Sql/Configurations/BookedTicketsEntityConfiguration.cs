using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.Theater;

namespace Theater.Sql.Configurations
{
    internal sealed class BookedTicketsEntityConfiguration : IEntityTypeConfiguration<BookedTicketEntity>
    {
        public void Configure(EntityTypeBuilder<BookedTicketEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.UserId, x.Id }).IsUnique();
        }
    }
}