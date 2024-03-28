namespace CloudStorage.Domain.Contracts.Folder;

public record FolderDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}