using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.Theater;

namespace Theater.Sql.Configurations
{
    internal class RepertoryWorkerEntityConfiguration : IEntityTypeConfiguration<RepertoryWorkerEntity>
    {
        public void Configure(EntityTypeBuilder<RepertoryWorkerEntity> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
