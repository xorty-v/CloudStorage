using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Persistence.EntityTypeConfiguration;

public class FileTypeConfiguration : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.HasIndex(f => f.Id);

        builder.Property(file => file.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(file => file.Size)
            .IsRequired();

        builder.Property(file => file.UploadDate)
            .IsRequired();
    }
}