using System.IO.Compression;

namespace CloudStorage.Persistence.Extensions;

public static class ZipArchiveExtensions
{
    public static bool IsFolder(this ZipArchiveEntry entry)
    {
        return entry.FullName.EndsWith("/");
    }
}