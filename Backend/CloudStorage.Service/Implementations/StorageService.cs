using System.IO.Compression;
using CloudStorage.Domain.Entities;
using CloudStorage.Domain.Interfaces;
using CloudStorage.Persistence.Extensions;
using CloudStorage.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Service.Implementations;

public class StorageService : IStorageService
{
    private readonly IStorageRepository _storageRepository;
    private readonly IFolderRepository _folderRepository;
    private readonly IFileRepository _fileRepository;

    public StorageService(IFolderRepository folderRepository, IFileRepository fileRepository,
        IStorageRepository storageRepository)
    {
        _folderRepository = folderRepository;
        _fileRepository = fileRepository;
        _storageRepository = storageRepository;
    }

    public async Task PutFolderAsync(IFormFile zipFile, Guid folderId, Guid userId, CancellationToken cancellationToken)
    {
        var folder = await _folderRepository.GetFolderContentByIdAsync(folderId, userId, cancellationToken);

        if (folder is null)
            throw new Exception("Folder was not found");

        using var zipArchive = new ZipArchive(zipFile.OpenReadStream(), ZipArchiveMode.Read);

        var currentFolder = folder;

        foreach (var entry in zipArchive.Entries)
        {
            if (entry.IsFolder())
                continue;

            var paths = entry.FullName.Split('/');
            currentFolder = folder;

            foreach (var folderName in paths.SkipLast(1))
            {
                var nextFolder = currentFolder.SubFolders.FirstOrDefault(f => f.Name == folderName);

                if (nextFolder == null)
                {
                    nextFolder = new Folder
                    {
                        Id = Guid.NewGuid(),
                        Name = folderName,
                        Files = new List<File?>(),
                        SubFolders = new List<Folder>()
                    };

                    currentFolder.SubFolders.Add(nextFolder);
                }

                currentFolder = nextFolder;
            }

            var file = new File
            {
                Id = Guid.NewGuid(),
                Name = entry.Name,
                Size = entry.Length,
                UploadDate = DateTime.UtcNow,
                FolderId = currentFolder.Id
            };

            currentFolder.Files.Add(file);
            await _storageRepository.PutFileAsync(entry.Open(), entry.Length, file.Id.ToString(),
                cancellationToken);
        }

        await _folderRepository.AddAsync(currentFolder, cancellationToken);
    }

    public async Task PutFilesAsync(IFormFileCollection files, Guid folderId, Guid userId,
        CancellationToken cancellationToken)
    {
        var folder = await _folderRepository.GetFolderContentByIdAsync(folderId, userId, cancellationToken);

        if (folder is null)
            throw new Exception("Folder was not found");

        foreach (var file in files)
        {
            if (folder.Files.Any(f => f.Name == file.FileName))
                continue;

            var newFile = new File
            {
                Id = Guid.NewGuid(),
                Name = file.FileName,
                Size = file.Length,
                UploadDate = DateTime.UtcNow,
                FolderId = folderId
            };

            await _fileRepository.AddAsync(newFile, cancellationToken);
            await _storageRepository.PutFileAsync(file.OpenReadStream(), file.Length, newFile.Id.ToString(),
                cancellationToken);
        }
    }
}