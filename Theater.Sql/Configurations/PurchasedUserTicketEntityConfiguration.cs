using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.Theater;

namespace Theater.Sql.Configurations
{
    internal sealed class PurchasedUserTicketEntityConfiguration : IEntityTypeConfiguration<PurchasedUserTicketEntity>
    {
        public void Configure(EntityTypeBuilder<PurchasedUserTicketEntity> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
