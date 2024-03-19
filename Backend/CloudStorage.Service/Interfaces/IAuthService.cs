using CloudStorage.Domain.Contracts;

namespace CloudStorage.Service.Interfaces;

public interface IAuthService
{
    public Task RegistrationAsync(string username, string email, string password, CancellationToken cancellationToken);
    public Task<TokenResponse> LoginAsync(string email, string password, CancellationToken cancellationToken);
}