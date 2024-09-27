using System.Linq.Expressions;
using System.Threading.Tasks.Dataflow;
using domain.Entities;
using domain.Enums;
using domain.Interfaces.Services;
using domain.Models;
using domain.Models.Request.Project;
using domain.Models.Request.Request;
using domain.Models.Request.User;
using domain.Models.Response.Request;
using domain.Utils;
using infra.Data.Repository;

namespace service.Services;

public class RequestService : IRequestService
{
    private readonly BaseRepository<Request> _requestRepositoryBase;
    private readonly BaseRepository<User> _userRepositoryBase;
    private readonly BaseRepository<Project> _projectRepositoryBase;
    private readonly StudentRepository _studentRepository;
    private readonly TeacherRepository _teacherRepository;
    private readonly RequestRepository _requestRepository;
    private readonly ProjectRepository _projectRepository;


    public RequestService(
        BaseRepository<Request> requestRepositoryBase, BaseRepository<User> userRepositoryBase,
        BaseRepository<Project> projectRepositoryBase, TeacherRepository teacherRepository,
        RequestRepository requestRepository, StudentRepository studentRepository,
        ProjectRepository projectRepository)
    {
        _requestRepositoryBase = requestRepositoryBase;
        _userRepositoryBase = userRepositoryBase;
        _projectRepositoryBase = projectRepositoryBase;
        _teacherRepository = teacherRepository;
        _requestRepository = requestRepository;
        _studentRepository = studentRepository;
        _projectRepository = projectRepository;
    }

    public async Task<ServiceResponse<string>> CreateRequest(CreateRequestModel body)
    {
        try
        {
            var user = await _userRepositoryBase.GetById(body.CreatorId);

            if (user == null)
            {
                return new ServiceResponse<string>
                {
                    Data = null,
                    Message = "Usuário criador não existe!",
                    Status = 404
                };
            };

            TypeRequestEnum requestType = 0;

            if (user.TypeUser == TypeUserEnum.Student) requestType = TypeRequestEnum.Orientation;
            if (user.TypeUser  == TypeUserEnum.Teacher) requestType = TypeRequestEnum.Evaluation;

            foreach (int userId in body.UsersId)
            {
                var request = new Request()
                {
                    ProjectId = body.ProjectId,
                    Type = requestType,
                    Status = userId == body.CreatorId ? StatusRequestEnum.Approval : StatusRequestEnum.Pending,
                    UserId = userId
                };

               await _requestRepositoryBase.Add(request);
            }

            return new ServiceResponse<string>()
            {
                Data = null,
                Message = "Requisições criadas",
                Status = 200
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<string>> CancelRequests(IProjectService projectService, int projectId,
        IEnumerable<Request> projectRequestsList, ChangeProjectStatus projectBody)
    {
        try
        {
            var serviceResponse = new ServiceResponse<string>();

            foreach (var projectRequest in projectRequestsList)
            {
                projectRequest.Status = StatusRequestEnum.Canceled;
                await _requestRepositoryBase.Update(projectRequest);
            }

            await projectService.ChangeProjectStatus(projectId, projectBody, null);

            serviceResponse.Status = 200;
            serviceResponse.Message = "Requisições canceladas com sucesso!";
            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<string>> ChangeStatusRequest(int requestId, ChangeStatusRequestModel body, IProjectService projectService, IUserService userService)
    {
        try
        {
            var serviceResponse = new ServiceResponse<string>();
            var changeProjectStatusBody = new ChangeProjectStatus();

            var request = await _requestRepositoryBase.GetById(
                requestId,
                includeProperties: new Expression<Func<Request, object>>[]
                {
                    x => x.Project,
                    x => x.User,
                });

            IEnumerable<Request> projectRequests = await _requestRepositoryBase.FindBy(x =>
                x.ProjectId == request.ProjectId);

            var projectRequestsList = projectRequests.ToList();

            if (projectRequestsList.Count == 0)
            {
                serviceResponse.Message = "Não existem requests para esse projeto!";
                serviceResponse.Status = 200;
                return serviceResponse;
            }

            if (body.Status == StatusRequestEnum.Denied && request.Type == TypeRequestEnum.Orientation)
            {
                changeProjectStatusBody.Status = ProjectStatus.Canceled;
                await CancelRequests(projectService, request.ProjectId, projectRequestsList, changeProjectStatusBody);
            }

            if (body.Status == StatusRequestEnum.Approval)
            {
                request.Status = body.Status;
                await _requestRepositoryBase.Update(request);
                request =  await _requestRepositoryBase.GetById(
                    requestId,
                    includeProperties: new Expression<Func<Request, object>>[]
                    {
                        x => x.Project,
                        x => x.User,
                    });

                IEnumerable<Request> updatedsProjectRequests = await _requestRepositoryBase.FindBy(
                    predicate: x => x.ProjectId == request.ProjectId,
                    includeProperties: new Expression<Func<Request, object>>[]
                    {
                        x => x.Project,
                        x => x.User,
                    });

                projectRequestsList = updatedsProjectRequests.ToList();


                var requestWithoutApproval = projectRequestsList.FirstOrDefault(requestItem => requestItem.Status != StatusRequestEnum.Approval);

                if (requestWithoutApproval == null && request.Type == TypeRequestEnum.Orientation)
                {
                    changeProjectStatusBody.Status = ProjectStatus.InProgress;
                    await projectService.ChangeProjectStatus(request.ProjectId, changeProjectStatusBody, null);

                    var projectStudents = await _requestRepository.GetProjectStudents(request.ProjectId);
                    var changeUserAvailableBody = new ChangeUserAvailableModel {Status = false};

                    if (projectStudents.Count == 0)
                    {
                        serviceResponse.Data = null;
                        serviceResponse.Message = "Ocorreu algum erro!";
                        serviceResponse.Status = 400;
                        return serviceResponse;
                    }

                    foreach (var projectStudent in projectStudents)
                    {
                        await userService.ChangeUserAvailable(projectStudent.Id, changeUserAvailableBody);
                    }

                    var peddingsProjects =
                        await _projectRepositoryBase.FindBy(
                            predicate: x => x.Status == ProjectStatus.PeddingApproval &&
                                            x.Requests.Any(y =>
                                                projectStudents.Any(student => student == y.User) &&
                                                x != request.Project ),
                            includeProperties: new Expression<Func<Project, object>>[]
                                            {
                                                x => x.Requests,
                                            });

                    foreach (var projectItem in peddingsProjects)
                    {
                        changeProjectStatusBody.Status = ProjectStatus.Canceled;
                        await CancelRequests(projectService, projectItem.Id, projectItem.Requests, changeProjectStatusBody);
                    }
                }
            }

            if (body.Status == StatusRequestEnum.Denied && request.Type == TypeRequestEnum.Evaluation)
            {
                var projectEvaluationRequests = projectRequestsList.Where(projectRequestItem =>
                    projectRequestItem.Type == TypeRequestEnum.Evaluation);

                foreach (var projectEvaluationRequest in projectEvaluationRequests)
                {
                    projectEvaluationRequest.Status = StatusRequestEnum.Canceled;
                    await _requestRepositoryBase.Update(projectEvaluationRequest);
                }
            }

            if (body.Status != StatusRequestEnum.Denied && body.Status != StatusRequestEnum.Approval)
            {
                request.Status = body.Status;
                await _requestRepositoryBase.Update(request);
            }

            serviceResponse.Message = "Status atualizado!";
            serviceResponse.Status = 200;
            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<GetOrientationRequestContext>> GetOrientationRequestContext(int userId)
    {
        try
        {
            var teachers = (await _teacherRepository.GetAllAvailableTeachersForOrientantion())
                .Select(x => new SelectListItem
                {
                    Id = x.UserId,
                    Label = x.User.Name
                });

            var students = (await _studentRepository.GetAllAvailableStudentsForOrientantion())
                .Select(x => new SelectListItem
                {
                    Id = x.Id,
                    Label = x.Name
                })
                .Where(x => x.Id != userId);

            var serviceResponse = new ServiceResponse<GetOrientationRequestContext>();

            serviceResponse.Status = 200;
            serviceResponse.Message = "Ok!";
            serviceResponse.Data = new GetOrientationRequestContext
            {
                Students = students,
                Teachers = teachers
            };

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<GetEvaluationRequestContext>> GetEvaluationRequestContext(int userId)
    {
        try
        {
            var teachers = (await _teacherRepository.GetAllAvailableTeachersForEvaluation())
                .Select(x => new SelectListItem
                {
                    Id = x.UserId,
                    Label = x.User.Name
                })
                .Where(x => x.Id != userId);

            var serviceResponse = new ServiceResponse<GetEvaluationRequestContext>();

            serviceResponse.Status = 200;
            serviceResponse.Message = "Ok!";
            serviceResponse.Data = new GetEvaluationRequestContext
            {
                Teachers = teachers
            };

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<IEnumerable<GetRequests>>> GetRequests(int userId)
    {
        try
        {
            var requests = (await _requestRepositoryBase
                .FindBy(predicate: x => x.Status == StatusRequestEnum.Pending && x.UserId == userId,
                    includeProperties: new Expression<Func<Request, object>>[]
                    {
                        x => x.User
                    }));

            var requestsInfos = new List<GetRequests>();
            var userInfos = requests.ToList().Count > 0 ? requests.FirstOrDefault().User : null;

            foreach (var request in requests)
            {
                var projectInfos = await _projectRepository.GetProjectInfos(request.ProjectId);
                requestsInfos.Add(new GetRequests
                {
                    Id = request.Id,
                    ProjectName = projectInfos.Name,
                    Description = projectInfos.Description,
                    RequestType = userInfos.TypeUser == TypeUserEnum.Student ? "Grupo" : Utils.GetEnumDescription(request.Type),
                    Students = projectInfos.Students
                });
            }

            var serviceResponse = new ServiceResponse<IEnumerable<GetRequests>>();

            serviceResponse.Status = 200;
            serviceResponse.Message = "Ok!";
            serviceResponse.Data = requestsInfos;

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
