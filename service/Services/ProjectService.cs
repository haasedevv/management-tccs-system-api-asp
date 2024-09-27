using System.Linq.Expressions;
using System.Xml.Linq;
using domain.Entities;
using domain.Enums;
using domain.Interfaces.Services;
using domain.Models.Request.Project;
using domain.Models.Request.Request;
using domain.Models.Request.User;
using domain.Models.Response.Project;
using domain.Validations.Request.Project;
using infra.Data.Repository;

namespace service.Services;

public class ProjectService : IProjectService
{
    private readonly BaseRepository<Project> _projectRepositoryBase;
    private readonly BaseRepository<ProjectTask> _projectTaskRepositoryBase;
    private readonly BaseRepository<Request> _requestRepositoryBase;
    private readonly ProjectRepository _projectRepository;
    private readonly RequestRepository _requestRepository;

    public ProjectService(BaseRepository<Project> projectRepositoryBase, ProjectRepository projectRepository,
        BaseRepository<ProjectTask> projectTaskRepositoryBase, BaseRepository<Request> requestRepositoryBase,
        RequestRepository requestRepository)
    {
        _projectRepositoryBase = projectRepositoryBase;
        _projectRepository = projectRepository;
        _projectTaskRepositoryBase = projectTaskRepositoryBase;
        _requestRepositoryBase = requestRepositoryBase;
        _requestRepositoryBase = requestRepositoryBase;
        _requestRepository = requestRepository;
    }

    public async Task<ServiceResponse<string>> CreateProject(CreateProjectModel body, IRequestService requestService)
    {
        var isValidBody = CreateProjectValidation.ValidateBody(body);

        if (!isValidBody)
        {
            return new ServiceResponse<string>
            {
                Status = 400,
                Message = "Body informado está incorreto!"
            };
        }

        try
        {
            var project = new Project
            {
                Name = body.Name,
                Description = body.Description,
                Status = 0,
                Created = DateTime.UtcNow,
            };

            await _projectRepositoryBase.Add(project);

            var createRequestBody = new CreateRequestModel()
            {
                ProjectId = project.Id,
                CreatorId = body.CreatorId,
                UsersId = body.Group.Concat([body.CreatorId, body.LeaderId]).ToList()
            };

            await  requestService.CreateRequest(createRequestBody);

            return new ServiceResponse<string>
            {
                Status = 200,
                Message = "Projeto Criado!"
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<string>> ChangeProjectStatus(int projectId, ChangeProjectStatus body, IUserService? userService)
    {
        try
        {
            var serviceResponse = new ServiceResponse<string>();
            var project = await _projectRepositoryBase.GetById(projectId);

            if (project == null)
            {
                serviceResponse.Data = null;
                serviceResponse.Message = "Projeto não encontrado!";
                serviceResponse.Status = 404;
                return serviceResponse;
            }

            project.Status = body.Status;
            await _projectRepositoryBase.Update(project);

            if (body.Status == ProjectStatus.Completed)
            {
                if (userService == null)
                {
                    serviceResponse.Data = null;
                    serviceResponse.Message = "Ocorreu um erro!";
                    serviceResponse.Status = 400;
                    return serviceResponse;
                }

                var projectStudents = await userService.GetProjectStudents(projectId);
                var changeUserAvailableBody = new ChangeUserAvailableModel {Status = true};

                if (projectStudents.Data == null)
                {
                    serviceResponse.Data = null;
                    serviceResponse.Message = "Ocorreu um erro!";
                    serviceResponse.Status = 400;
                    return serviceResponse;
                }

                foreach (var student in projectStudents.Data)
                {
                    await userService.ChangeUserAvailable(student.Id, changeUserAvailableBody);
                }
            }

            serviceResponse.Data = null;
            serviceResponse.Message = "Status do projeto atualizado!";
            serviceResponse.Status = 200;

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<GetTasks>> GetTasks(int projectId)
    {
        try
        {
            var serviceResponse = new ServiceResponse<GetTasks>();
            var tasks = (await _projectTaskRepositoryBase.FindBy(x => x.ProjectId == projectId))
                .Select(x => new ResumedTask
                {
                    Id = x.Id,
                    Title = x.Title,
                    Completed =  x.Completed
                });

            serviceResponse.Status = 200;
            serviceResponse.Message = "OK!";
            serviceResponse.Data = new GetTasks
            {
                Tasks = tasks
            };

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<int>> CheckHasProjectInProgressOfTheUser(int userId)
    {
        try
        {
            var serviceResponse = new ServiceResponse<int>();
            var request = (await _requestRepositoryBase.FindBy(x =>
                x.UserId == userId && x.Project.Status == ProjectStatus.InProgress)).FirstOrDefault();

            if (request == null)
            {
                serviceResponse.Status = 404;
                serviceResponse.Message = "Não há projeto ativo!";
                return serviceResponse;
            }

            serviceResponse.Status = 200;
            serviceResponse.Message = "OK!";
            serviceResponse.Data = request.ProjectId;

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<GetProjectsOfTheOrientation>> GetProjectsOfTheOrientation(int userId)
    {
        try
        {
            var serviceResponse = new ServiceResponse<GetProjectsOfTheOrientation>();
            var projectsIds = (await _requestRepositoryBase
                    .FindBy(
                        predicate: x =>
                        x.UserId == userId && x.Project.Status == ProjectStatus.InProgress &&
                        x.Type == TypeRequestEnum.Orientation,
                        includeProperties: new Expression<Func<Request, object>>[]
                        {
                            x => x.Project
                        }))
                .Select(x => x.ProjectId);

            var projects = new List<ResumedProject>();

            foreach (var projectId in projectsIds)
            {
                    var project = await _projectRepository.GetProjectInfos((projectId));

                    projects.Add(new ResumedProject
                    {
                        Id = project.Id,
                        Name = project.Name,
                        Students = project.Students.Select(x => x.Name)
                    });
            }

            serviceResponse.Status = 200;
            serviceResponse.Message = "OK!";
            serviceResponse.Data = new GetProjectsOfTheOrientation()
            {
                Projects = projects
            };

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<GetProjectsOfTheOrientation>> GetProjectsOfTheEvaluation(int userId)
    {
        try
        {
            var serviceResponse = new ServiceResponse<GetProjectsOfTheOrientation>();
            var projectIds = (await _requestRepositoryBase
                .FindBy(
                    predicate: x =>
                        x.UserId == userId && x.Project.Status == ProjectStatus.InProgress &&
                        x.Type == TypeRequestEnum.Evaluation,
                    includeProperties: new Expression<Func<Request, object>>[]
                    {
                        x => x.Project
                    }))
                .Select(x => x.ProjectId)
                .ToList();

            if (projectIds.Count == 0)
            {
                serviceResponse.Status = 200;
                serviceResponse.Message = "Não tem projeto para avaliar!";
                serviceResponse.Data = new GetProjectsOfTheOrientation()
                {
                    Projects = []
                };

                return serviceResponse;
            }

            var projects = new List<Project>();

            foreach (var projectId in projectIds)
            {
                var project = (await _projectRepositoryBase
                    .FindBy(
                        predicate: x => x.Id == projectId,
                        includeProperties: new Expression<Func<Project, object>>[]
                        {
                            x => x.Requests
                        })).FirstOrDefault();

                projects.Add(project);
            }


            var projectsInEvaluations = new List<int>();

            foreach (var project in projects)
            {
                var evaluationRequests = project.Requests.Where(x => x.Type == TypeRequestEnum.Evaluation);
                var hasPenddingRequest = evaluationRequests.Where(x => x.Status != StatusRequestEnum.Approval).ToList();

                if (hasPenddingRequest.Count == 0)
                {
                    projectsInEvaluations.Add(project.Id);
                }
            }

            if (projectsInEvaluations.Count == 0)
            {
                serviceResponse.Status = 200;
                serviceResponse.Message = "Não tem projeto para avaliar!";
                serviceResponse.Data = new GetProjectsOfTheOrientation()
                {
                    Projects = []
                };

                return serviceResponse;
            }

            var resumedProjects = new List<ResumedProject>();

            foreach (var projectId in projectsInEvaluations)
            {
                var project = await _projectRepository.GetProjectInfos((projectId));

                resumedProjects.Add(new ResumedProject
                {
                    Id = project.Id,
                    Name = project.Name,
                    Students = project.Students.Select(x => x.Name),
                    Leaders = project.teacherAdvisor.Select(x => x.Name)
                });
            }

            serviceResponse.Status = 200;
            serviceResponse.Message = "OK!";
            serviceResponse.Data = new GetProjectsOfTheOrientation()
            {
                Projects = resumedProjects
            };

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<ProjectInfos>> GetProjectInfos(int projectId)
    {
        try
        {
            var serviceResponse = new ServiceResponse<ProjectInfos>();
            var project = await _projectRepository.GetProjectInfos(projectId);

            if (project == null)
            {
                serviceResponse.Status = 404;
                serviceResponse.Message = "Projeto não encontrado!";
                serviceResponse.Data = null;
                return serviceResponse;
            }

            serviceResponse.Status = 200;
            serviceResponse.Message = "OK!";
            serviceResponse.Data = project;

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
