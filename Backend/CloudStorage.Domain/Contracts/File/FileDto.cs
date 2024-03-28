namespace CloudStorage.Domain.Contracts.File;

public class FileDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public long Size { get; set; }
}