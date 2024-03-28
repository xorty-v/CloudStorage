using CloudStorage.Domain.Interfaces;
using CloudStorage.Persistence.Extensions;
using CloudStorage.Service.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Service.Implementations;

public class FileService : IFileService
{
    private readonly IStorageRepository _storageRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IFolderRepository _folderRepository;


    public FileService(IStorageRepository storageRepository, IFileRepository fileRepository,
        IFolderRepository folderRepository)
    {
        _storageRepository = storageRepository;
        _fileRepository = fileRepository;
        _folderRepository = folderRepository;
    }

    public async Task PutFilesAsync(Stream fileStream, Guid folderId, Guid userId, string contentType,
        CancellationToken cancellationToken)
    {
        var folder = await _folderRepository.GetFolderContentByIdAsync(folderId, userId, cancellationToken);

        if (folder is null)
            throw new Exception("Folder was not found");

        var boundary = MediaTypeHeaderValue.Parse(contentType).GetBoundary();
        var multipartReader = new MultipartReader(boundary, fileStream);
        MultipartSection? section;

        while ((section = await multipartReader.ReadNextSectionAsync(cancellationToken)) != null)
        {
            var fileSection = section.AsFileSection();

            if (fileSection == null)
                continue;

            var fileId = Guid.NewGuid();

            await _storageRepository.PutFileAsync(fileSection.FileStream, -1, fileId.ToString(), cancellationToken);

            var newFile = new File
            {
                Id = fileId,
                Name = fileSection.FileName,
                Size = fileSection.FileStream.Length,
                UploadDate = DateTime.UtcNow,
                FolderId = folderId
            };
            await _fileRepository.AddAsync(newFile, cancellationToken);
        }
    }

    public Task DownloadFilesAsync(Guid fileId, Func<Stream, CancellationToken, Task> fileStreamCallback,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}