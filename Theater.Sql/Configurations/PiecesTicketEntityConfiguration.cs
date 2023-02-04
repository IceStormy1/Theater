using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.Theater;

namespace Theater.Sql.Configurations
{
    internal sealed class PiecesTicketEntityConfiguration : IEntityTypeConfiguration<PiecesTicketEntity>
    {
        public void Configure(EntityTypeBuilder<PiecesTicketEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(s => s.BookedTickets)
                .WithOne(x => x.PiecesTicket)
                .HasForeignKey(x => x.PiecesTicketId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.TicketPriceEvents)
                .WithOne(x => x.PiecesTicket)
                .HasForeignKey(x => x.PiecesTicketId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
