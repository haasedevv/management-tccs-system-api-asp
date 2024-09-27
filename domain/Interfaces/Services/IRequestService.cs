using domain.Entities;
using domain.Enums;
using domain.Models.Request.Request;
using domain.Models.Response.Request;

namespace domain.Interfaces.Services;

public interface IRequestService
{
    Task<ServiceResponse<string>> CreateRequest(CreateRequestModel body);
    Task<ServiceResponse<string>> ChangeStatusRequest(int requestId, ChangeStatusRequestModel status, IProjectService projectService, IUserService userService);
    Task<ServiceResponse<GetOrientationRequestContext>> GetOrientationRequestContext(int userId);
    Task<ServiceResponse<GetEvaluationRequestContext>> GetEvaluationRequestContext(int userId);
    Task<ServiceResponse<IEnumerable<GetRequests>>> GetRequests(int userId);
}
