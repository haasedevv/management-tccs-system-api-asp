using domain.Entities;
using domain.Enums;
using domain.Models.Response.Project;
using infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace infra.Data.Repository;

public class ProjectRepository
{
    protected readonly NpSqlContext _npSqlContext;

    public ProjectRepository(NpSqlContext npSqlContext)
    {
        _npSqlContext = npSqlContext;
    }

    public async Task<List<Request>> GetProjectsOfTheEvaluation(int userId)
    {
        var requests = await _npSqlContext.Set<Request>()
            .Where(x =>  x.UserId == userId &&
                         x.Project.Status == ProjectStatus.InProgress &&
                         x.Type == TypeRequestEnum.Evaluation)
            .Include(x => x.Project)
            .ThenInclude(x => x.Requests)
            .AsNoTracking()
            .ToListAsync();

        return requests;
    }


    public async Task<ProjectInfos> GetProjectInfos(int projectId)
    {

        var project =  await _npSqlContext.Set<Project>()
            .Where(x => x.Id == projectId)
            .Include(x => x.Requests)
            .ThenInclude(x => x.User)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (project == null)
        {
            return null;
        }

        var orientationRequests = project?.Requests.Where(x => x.Type == TypeRequestEnum.Orientation);
        var evaluationRequests = project?.Requests.Where(x => x.Type == TypeRequestEnum.Evaluation);

        var students = orientationRequests
            .Where(x => x.User.TypeUser == TypeUserEnum.Student)
            .Select(x => new UserInfos
            {
                Id = x.User.Id,
                Name = x.User.Name,
                Enrollment = x.User.Enrollment
            });

        var teacherAdvisor = orientationRequests
            .Where(x => x.User.TypeUser == TypeUserEnum.Teacher)
            .Select(x => new UserInfos
            {
                Id = x.User.Id,
                Name = x.User.Name,
                Enrollment = x.User.Enrollment
            });

        var teacherEvaluators = evaluationRequests
            .Where(x => x.User.TypeUser == TypeUserEnum.Teacher)
            .Select(x => new UserInfos
            {
                Id = x.User.Id,
                Name = x.User.Name,
                Enrollment = x.User.Enrollment
            });


        return new ProjectInfos
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Status = project.Status,
            Students = students,
            teacherAdvisor = teacherAdvisor,
            teacherEvaluators = teacherEvaluators
        };
    }
}
