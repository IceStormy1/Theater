using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Theater.Entities.FileStorage;

namespace Theater.Sql.Configurations
{
    internal class FileStorageEntityConfiguration : IEntityTypeConfiguration<FileStorageEntity>
    {
        public void Configure(EntityTypeBuilder<FileStorageEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.FileName).IsRequired().HasMaxLength(128);
            builder.Property(x => x.FileName).IsRequired();
        }
    }
}
