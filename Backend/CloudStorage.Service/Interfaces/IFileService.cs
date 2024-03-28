namespace CloudStorage.Service.Interfaces;

public interface IFileService
{
    public Task PutFilesAsync(Stream fileStream, Guid folderId, Guid userId, string contentType,
        CancellationToken cancellationToken);

    public Task DownloadFilesAsync(Guid fileId, Func<Stream, CancellationToken, Task> fileStreamCallback,
        CancellationToken cancellationToken);
}