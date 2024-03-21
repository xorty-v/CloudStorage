using CloudStorage.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : BaseController
{
    private readonly IStorageService _storageService;

    public FileController(IStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpPost("put-files")]
    [Authorize]
    public async Task<IActionResult> PutFiles(IFormFileCollection files, Guid folderId,
        CancellationToken cancellationToken)
    {
        await _storageService.PutFilesAsync(files, folderId, UserId, cancellationToken);

        return Ok();
    }
}