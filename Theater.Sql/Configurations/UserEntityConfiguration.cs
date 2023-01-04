using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.Authorization;

namespace Theater.Sql.Configurations
{
    internal class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.UserName).IsRequired().HasMaxLength(128);
            builder.Property(x=>x.Email).IsRequired().HasMaxLength(256);
            builder.Property(x=>x.Phone).IsRequired().HasMaxLength(11).IsFixedLength();
            builder.Property(x=>x.FirstName).IsRequired().HasMaxLength(128);
            builder.Property(x=>x.LastName).IsRequired().HasMaxLength(128);
            builder.Property(x=>x.MiddleName).HasMaxLength(128);
        }
    }
}
