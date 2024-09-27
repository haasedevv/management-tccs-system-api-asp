using domain.Entities;
using domain.Models.Request.Request;
using domain.Models.Response.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace application.Controllers;

[Authorize]
[Route("api/request")]
[ApiController]
public class RequestController : ControllerBase
{
    private RequestService _requestService;
    private ProjectService _projectService;
    private UserService _userService;
    private TeacherService _teacherService;

    public RequestController(RequestService requestService, ProjectService projectService, UserService userService)
    {
        _requestService = requestService;
        _projectService = projectService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<string>>> CreateRequest([FromBody] CreateRequestModel body) {
        var response = await _requestService.CreateRequest(body);
        return StatusCode(response.Status, response);
    }

    [HttpPut]
    [Route("{requestId}/status")]
    public async Task<ActionResult<ServiceResponse<string>>> ChangeStatusRequest([FromRoute] int requestId, [FromBody] ChangeStatusRequestModel body) {
        var response = await _requestService.ChangeStatusRequest(requestId, body, _projectService, _userService);
        return StatusCode(response.Status, response);
    }

    [HttpGet]
    [Route("orientation/context/{userId}")]
    public async Task<ActionResult<ServiceResponse<GetOrientationRequestContext>>> GetOrientationRequestContext([FromRoute] int userId)
    {
        var response = await _requestService.GetOrientationRequestContext(userId);
        return StatusCode(response.Status, response);
    }

    [HttpGet]
    [Route("evaluation/context/{userId}")]
    public async Task<ActionResult<ServiceResponse<GetOrientationRequestContext>>> GetEvaluationRequestContext([FromRoute] int userId)
    {
        var response = await _requestService.GetEvaluationRequestContext(userId);
        return StatusCode(response.Status, response);
    }

    [HttpGet]
    [Route("pendding-list/{userId}")]
    public async Task<ActionResult<ServiceResponse<GetOrientationRequestContext>>> GetRequests([FromRoute] int userId)
    {
        var response = await _requestService.GetRequests(userId);
        return StatusCode(response.Status, response);
    }
}
