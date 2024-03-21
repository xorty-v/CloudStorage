namespace CloudStorage.Domain.Interfaces;

public interface IStorageRepository
{
    public Task PutFileAsync(Stream streamFile, long length, string objectName, CancellationToken cancellationToken);

    public Task GetFileAsync(string objectName, Func<Stream, CancellationToken, Task> fileStreamCallback,
        CancellationToken cancellationToken);
}