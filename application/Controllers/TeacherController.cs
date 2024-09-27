using domain.Entities;
using domain.Models.Request.Request;
using domain.Models.Request.Teacher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace application.Controllers;

[Authorize]
[Route("api/teacher")]
[ApiController]
public class TeacherController : ControllerBase
{
    private TeacherService _teacherService;

    public TeacherController(TeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpPut]
    [Route("orientation-available/{userId}")]
    public async Task<ActionResult<ServiceResponse<string>>> ChangeOrientationAvailable([FromRoute] int userId, [FromBody] ChangeOrientationAvailableModel body)
    {
        var response = await _teacherService.ChangeOrientationAvailable(userId, body.Status);
        return StatusCode(response.Status, response);
    }

    [HttpPut]
    [Route("evaluation-available/{userId}")]
    public async Task<ActionResult<ServiceResponse<string>>> ChangeEvaluationAvailable([FromRoute] int userId, [FromBody] ChangeEvaluationAvailableModel body)
    {
        var response = await _teacherService.ChangeEvaluationAvailable(userId, body.Status);
        return StatusCode(response.Status, response);
    }
}
