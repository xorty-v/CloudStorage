using CloudStorage.Domain.Contracts.File;

namespace CloudStorage.Domain.Contracts.Folder;

public class GetFolderResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<FolderDto>? Folders { get; set; }
    public IEnumerable<FileDto>? Files { get; set; }
}