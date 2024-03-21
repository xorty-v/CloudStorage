using CloudStorage.Domain.Entities;

namespace CloudStorage.Domain.Interfaces;

public interface IFolderRepository
{
    public Task AddAsync(Folder folder, CancellationToken cancellationToken);
    public Task InitUserRootDirectory(Guid userId, CancellationToken cancellationToken);
    public Task<Folder?> GetFolderContentByIdAsync(Guid folderId, Guid userId, CancellationToken cancellationToken);
}