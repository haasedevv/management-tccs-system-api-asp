using domain.Entities;
using domain.Models.Request.Auth;
using domain.Models.Response.Auth;
using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace application.Controllers;

[Route("api/auth/login")]
[ApiController]
public class AuthController : ControllerBase
{
    private AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<LoginResponseModel>>> Login([FromBody] LoginRequestModel body)
    {
        var response = await _authService.Login(body);
        return StatusCode(response.Status, response);
    }
}
