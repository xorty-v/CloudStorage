using CloudStorage.Domain.Constants;
using CloudStorage.Domain.Contracts;
using CloudStorage.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await _authService.LoginAsync(request.Email, request.Password, cancellationToken);

        HttpContext.Response.Cookies.Append(JwtConstants.ACCESS_TOKEN_COOKIE, response.AccessToken);

        return Ok();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Registration(RegistrationRequest request, CancellationToken cancellationToken)
    {
        await _authService.RegistrationAsync(request.Username, request.Email, request.Password, cancellationToken);

        return Ok();
    }
}