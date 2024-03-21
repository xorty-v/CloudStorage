using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Domain.Interfaces;

public interface IFileRepository
{
    public Task AddAsync(File file, CancellationToken cancellationToken);
    public Task<File?> GetById(Guid fileId, Guid userId, CancellationToken cancellationToken);
}