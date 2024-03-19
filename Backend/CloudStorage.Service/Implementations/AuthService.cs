using System.Security.Claims;
using CloudStorage.Domain.Contracts;
using CloudStorage.Domain.Entities;
using CloudStorage.Domain.Interfaces;
using CloudStorage.Persistence.Extensions;
using CloudStorage.Persistence.Helpers;
using CloudStorage.Service.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CloudStorage.Service.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task RegistrationAsync(string username, string email, string password,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is not null)
            throw new Exception("User with this email already exists");

        user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email,
            PasswordHash = _passwordHasher.Generate(password)
        };

        await _userRepository.AddAsync(user, cancellationToken);
    }

    public async Task<TokenResponse> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
            throw new Exception("User was not found");

        var isValidPassword = _passwordHasher.Verify(password, user.PasswordHash);

        if (!isValidPassword)
            throw new Exception("Invalid password");

        Claim[] claims = { new("userId", user.Id.ToString()) };
        var accessToken = _configuration.GenerateAccessToken(claims);

        return new TokenResponse(accessToken);
    }
}