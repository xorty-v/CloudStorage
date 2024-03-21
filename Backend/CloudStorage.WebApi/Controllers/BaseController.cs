using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BaseController : ControllerBase
{
    internal Guid UserId => User.Identity!.IsAuthenticated
        ? Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value)
        : Guid.Empty;
}