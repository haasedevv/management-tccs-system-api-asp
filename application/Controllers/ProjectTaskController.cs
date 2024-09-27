using domain.Entities;
using domain.Models.Request.Task;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace application.Controllers;


[Authorize]
[Route("api/task")]
[ApiController]
public class ProjectTaskController : ControllerBase
{
    private ProjectTaskService _projectTaskService;

    public ProjectTaskController(ProjectTaskService projectTaskService)
    {
        _projectTaskService = projectTaskService;
    }

    [HttpPost]
    [Route("{projectId}/{userId}")]
    public async Task<ActionResult<ServiceResponse<string>>> CreateProjectTask([FromRoute] int userId, [FromRoute] int projectId, [FromBody] CreateTaskModel body)
    {
        var response = await _projectTaskService.CreateProjectTask(userId, projectId, body);
        return StatusCode(response.Status, response);
    }

    [HttpGet]
    [Route("{taskId}")]
    public async Task<ActionResult<ServiceResponse<string>>> GetTask([FromRoute] int taskId)
    {
        var response = await _projectTaskService.GetTask(taskId);
        return StatusCode(response.Status, response);
    }

    [HttpPost]
    [Route("{taskId}/attachment")]
    public async Task<ActionResult<ServiceResponse<string>>> AddAttachment([FromRoute] int taskId, [FromBody] AddAttachmentModel body)
    {
        var response = await _projectTaskService.AddAttachment(taskId, body);
        return StatusCode(response.Status, response);
    }

    [HttpPost]
    [Route("{taskId}/comment")]
    public async Task<ActionResult<ServiceResponse<string>>> AddComment([FromRoute] int taskId, [FromBody] AddCommentModel body)
    {
        var response = await _projectTaskService.AddComment(taskId, body);
        return StatusCode(response.Status, response);
    }

    [HttpPut]
    [Route("{taskId}/complete")]
    public async Task<ActionResult<ServiceResponse<string>>> CompleteTask([FromRoute] int taskId)
    {
        var response = await _projectTaskService.CompleteTask(taskId);
        return StatusCode(response.Status, response);
    }
}
