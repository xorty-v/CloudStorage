namespace CloudStorage.Domain.Entities;

public class File
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public long Size { get; set; }
    public DateTime UploadDate { get; set; }
    public Guid FolderId { get; set; }
    public Folder Folder { get; set; }
}