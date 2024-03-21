using CloudStorage.Service.Implementations;
using CloudStorage.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudStorage.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IStorageService, StorageService>();

        return services;
    }
}