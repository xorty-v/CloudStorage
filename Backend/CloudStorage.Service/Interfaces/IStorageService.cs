using Microsoft.AspNetCore.Http;

namespace CloudStorage.Service.Interfaces;

public interface IStorageService
{
    public Task PutFolderAsync(IFormFile zipFile, Guid folderId, Guid userId, CancellationToken cancellationToken);

    public Task PutFilesAsync(IFormFileCollection files, Guid folderId, Guid userId,
        CancellationToken cancellationToken);
}