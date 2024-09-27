using domain.Entities;
using domain.Models.Request.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace application.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<string>>> AddUser([FromBody] CreateUserModel body)
    {
        var response = await _userService.AddUser(body);
        return StatusCode(response.Status, response);
    }

    [Authorize]
    [HttpPut]
    [Route("available/{id}")]
    public async Task<ActionResult<ServiceResponse<string>>> ChangeUserAvailable(
        [FromRoute] int id,
        [FromBody] ChangeUserAvailableModel body)
    {
        var response = await _userService.ChangeUserAvailable(id, body);
        return StatusCode(response.Status, response);
    }
}
