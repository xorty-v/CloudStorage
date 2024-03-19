using CloudStorage.Domain.Entities;
using CloudStorage.Persistence.EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;
using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.Migrate();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<File> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserTypeConfiguration());
        builder.ApplyConfiguration(new FolderTypeConfiguration());
        builder.ApplyConfiguration(new FileTypeConfiguration());

        base.OnModelCreating(builder);
    }
}