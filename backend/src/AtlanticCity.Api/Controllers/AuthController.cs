using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AtlanticCity.Application.DTOs;
using AtlanticCity.Application.Interfaces;

namespace AtlanticCity.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(response);
    }
}
