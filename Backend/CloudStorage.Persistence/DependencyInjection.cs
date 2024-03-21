using CloudStorage.Domain.Interfaces;
using CloudStorage.Persistence.Helpers;
using CloudStorage.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace CloudStorage.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSql")));

        services.AddMinio(options => options
            .WithEndpoint(configuration["MinioOptions:Endpoint"])
            .WithCredentials(configuration["MinioOptions:AccessKey"], configuration["MinioOptions:SecretKey"])
            .WithSSL(false)
            .Build());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IFolderRepository, FolderRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IStorageRepository, StorageRepository>();

        return services;
    }
}