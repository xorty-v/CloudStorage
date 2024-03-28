using CloudStorage.Domain.Entities;
using CloudStorage.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.Persistence.Repositories;

public class FolderRepository : IFolderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FolderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Folder folder, CancellationToken cancellationToken)
    {
        await _dbContext.Folders.AddAsync(folder, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task InitUserRootDirectory(Guid userId, CancellationToken cancellationToken)
    {
        var folder = new Folder
        {
            UserId = userId,
            Name = "Root"
        };

        await AddAsync(folder, cancellationToken);
    }

    public async Task<Folder?> GetFolderContentByIdAsync(Guid folderId, Guid userId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Folders
            .AsNoTracking()
            .Include(f => f.SubFolders)
            .Include(f => f.Files)
            .FirstOrDefaultAsync(f => f.Id == folderId && f.UserId == userId, cancellationToken);
    }
}