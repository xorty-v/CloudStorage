using CloudStorage.Domain.Entities;

namespace CloudStorage.Domain.Interfaces;

public interface IUserRepository
{
    public Task AddAsync(User user, CancellationToken cancellationToken);
    public Task UpdateAsync(User user, CancellationToken cancellationToken);
    public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}