using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CloudStorage.Persistence.Extensions;

public static class JwtBearerExtension
{
    private static SigningCredentials CreateSigningCredentials(this IConfiguration configuration)
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"]!)
            ),
            SecurityAlgorithms.HmacSha256
        );
    }

    public static string GenerateAccessToken(this IConfiguration configuration, Claim[] authClaims)
    {
        var tokenExpire = configuration.GetSection("JwtOptions:AccessTokenExpires").Get<int>();

        var token = new JwtSecurityToken(
            signingCredentials: configuration.CreateSigningCredentials(),
            issuer: configuration["JwtOptions:Issuer"],
            audience: configuration["JwtOptions:Audience"],
            claims: authClaims,
            expires: DateTime.UtcNow.AddMinutes(tokenExpire));

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}