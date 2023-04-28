using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.Theater;

namespace Theater.Sql.Configurations;

internal sealed class UserReviewEntityConfiguration : IEntityTypeConfiguration<UserReviewEntity>
{
    public void Configure(EntityTypeBuilder<UserReviewEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(1024);
    }
}