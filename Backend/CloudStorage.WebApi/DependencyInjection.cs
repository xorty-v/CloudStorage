using System.Text;
using CloudStorage.Domain.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CloudStorage.WebApi;

public static class DependencyInjection
{
    public static void AddApiAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var secretKey = Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"]);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[JwtConstants.ACCESS_TOKEN_COOKIE];

                        return Task.CompletedTask;
                    }
                };
            });
    }
}