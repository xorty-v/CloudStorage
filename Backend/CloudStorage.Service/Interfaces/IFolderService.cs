using CloudStorage.Domain.Contracts.Folder;
using Microsoft.AspNetCore.Http;

namespace CloudStorage.Service.Interfaces;

public interface IFolderService
{
    public Task CreateFolderAsync(string folderName, Guid folderId, Guid userId, CancellationToken cancellationToken);
    public Task PutFolderAsync(IFormFile zipFile, Guid folderId, Guid userId, CancellationToken cancellationToken);
    public Task<GetFolderResponse> GetFolderContentAsync(Guid folderId, Guid userId, CancellationToken token);
}