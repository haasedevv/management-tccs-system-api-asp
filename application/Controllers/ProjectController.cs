using domain.Entities;
using domain.Enums;
using domain.Models.Request.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace application.Controllers;

[Authorize]
[Route("api/project")]
[ApiController]
public class ProjectController : ControllerBase
{
    private RequestService _requestService;
    private ProjectService _projectService;
    private UserService _userService;

    public ProjectController(ProjectService projectService, RequestService requestService, UserService userService)
    {
        _projectService = projectService;
        _requestService = requestService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<string>>> CreateProject([FromBody] CreateProjectModel body){
        Console.WriteLine(body.Name);
        var response = await _projectService.CreateProject(body, _requestService);
        return StatusCode(response.Status, response);
    }

    [HttpPut]
    [Route("{projectId}/status")]
    public async Task<ActionResult<ServiceResponse<string>>> ChangeProjectStatus([FromRoute] int projectId, [FromBody] ChangeProjectStatus body){
        var response = await _projectService.ChangeProjectStatus(projectId, body, _userService);
        return StatusCode(response.Status, response);
    }

    [HttpGet]
    [Route("{projectId}/tasks")]
    public async Task<ActionResult<ServiceResponse<string>>> GetTasks([FromRoute] int projectId) {
        var response = await _projectService.GetTasks(projectId);
        return StatusCode(response.Status, response);

    }

    [HttpGet]
    [Route("{userId}/verify")]
    public async Task<ActionResult<ServiceResponse<string>>> CheckHasProjectInProgressOfTheUser([FromRoute] int userId) {
        var response = await _projectService.CheckHasProjectInProgressOfTheUser(userId);
        return StatusCode(response.Status, response);
    }

    [HttpGet]
    [Route("{userId}/orientations")]
    public async Task<ActionResult<ServiceResponse<string>>> GetProjectsOfTheOrientationTask([FromRoute] int userId) {
        var response = await _projectService.GetProjectsOfTheOrientation(userId);
        return StatusCode(response.Status, response);
    }

    [HttpGet]
    [Route("{userId}/evaluations")]
    public async Task<ActionResult<ServiceResponse<string>>> GetProjectsOfTheEvaluation([FromRoute] int userId) {
        var response = await _projectService.GetProjectsOfTheEvaluation(userId);
        return StatusCode(response.Status, response);
    }

    [HttpGet]
    [Route("{projectId}")]
    public async Task<ActionResult<ServiceResponse<string>>> GetProjectInfos([FromRoute] int projectId) {
        var response = await _projectService.GetProjectInfos(projectId);
        return StatusCode(response.Status, response);
    }
}
