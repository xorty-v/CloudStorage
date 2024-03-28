namespace CloudStorage.Domain.Contracts.Folder;

public record CreateFolderRequest(string FolderName, Guid FolderId);