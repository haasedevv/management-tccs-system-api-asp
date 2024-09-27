using domain.Entities;
using domain.Enums;
using domain.Models.Request.Project;

namespace domain.Interfaces.Services;

public interface IProjectService
{
    Task<ServiceResponse<string>> CreateProject(CreateProjectModel body, IRequestService requestService);
    Task<ServiceResponse<string>> ChangeProjectStatus(int projectId, ChangeProjectStatus body, IUserService? userService);
}
