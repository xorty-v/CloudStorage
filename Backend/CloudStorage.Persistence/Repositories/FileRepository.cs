using CloudStorage.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = CloudStorage.Domain.Entities.File;

namespace CloudStorage.Persistence.Repositories;

public class FileRepository : IFileRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FileRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(File file, CancellationToken cancellationToken)
    {
        await _dbContext.Files.AddAsync(file, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<File?> GetById(Guid fileId, Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Files
            .AsNoTracking()
            .Include(f => f.Folder)
            .FirstOrDefaultAsync(f => f.Id == fileId && f.Folder.UserId == userId, cancellationToken);
    }
}