using System.IO.Compression;
using CloudStorage.Domain.Contracts.File;
using CloudStorage.Domain.Contracts.Folder;
using CloudStorage.Domain.Entities;
using CloudStorage.Domain.Interfaces;
using CloudStorage.Persistence.Extensions;
using CloudStorage.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Service.Implementations;

public class FolderService : IFolderService
{
    private readonly IFolderRepository _folderRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IStorageRepository _storageRepository;

    public FolderService(IFolderRepository folderRepository, IStorageRepository storageRepository,
        IFileRepository fileRepository)
    {
        _folderRepository = folderRepository;
        _storageRepository = storageRepository;
        _fileRepository = fileRepository;
    }

    public async Task CreateFolderAsync(string folderName, Guid folderId, Guid userId,
        CancellationToken cancellationToken)
    {
        var folder = await _folderRepository.GetFolderContentByIdAsync(folderId, userId, cancellationToken);

        if (folder is null)
            throw new Exception("Folder was not found");

        var newFolder = new Folder
        {
            Name = folderName,
            FolderId = folderId,
            UserId = userId
        };

        await _folderRepository.AddAsync(newFolder, cancellationToken);
    }

    public async Task PutFolderAsync(IFormFile zipFile, Guid folderId, Guid userId,
        CancellationToken cancellationToken)
    {
        var folder = await _folderRepository.GetFolderContentByIdAsync(folderId, userId, cancellationToken);

        if (folder is null)
            throw new Exception("Folder was not found");

        using var zipArchive = new ZipArchive(zipFile.OpenReadStream(), ZipArchiveMode.Read);

        foreach (var entry in zipArchive.Entries)
        {
            if (entry.IsFolder())
                continue;

            var paths = entry.FullName.Split('/');
            var currentFolder = folder;

            foreach (var folderName in paths.SkipLast(1))
            {
                var nextFolder = currentFolder.SubFolders.FirstOrDefault(f => f.Name == folderName);

                if (nextFolder == null)
                {
                    nextFolder = new Folder
                    {
                        Id = Guid.NewGuid(),
                        FolderId = currentFolder.Id,
                        UserId = userId,
                        Name = folderName,
                        Files = new List<File?>(),
                        SubFolders = new List<Folder>()
                    };

                    currentFolder.SubFolders.Add(nextFolder);
                    await _folderRepository.AddAsync(nextFolder, cancellationToken);
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

            await _fileRepository.AddAsync(file, cancellationToken);
            await _storageRepository.PutFileAsync(entry.Open(), entry.Length, file.Id.ToString(), cancellationToken);
        }
    }

    public async Task<GetFolderResponse> GetFolderContentAsync(Guid folderId, Guid userId,
        CancellationToken cancellationToken)
    {
        var folderContent = await _folderRepository.GetFolderContentByIdAsync(folderId, userId, cancellationToken);

        if (folderContent is null)
            throw new Exception("Folder was not found");

        var response = new GetFolderResponse
        {
            Id = folderContent.Id,
            Name = folderContent.Name,
            Folders = folderContent.SubFolders.Select(f => new FolderDto
            {
                Id = f.Id,
                Name = f.Name
            }),
            Files = folderContent.Files.Select(f => new FileDto
            {
                Id = f.Id,
                Name = f.Name,
                Size = f.Size
            })
        };

        return response;
    }
}