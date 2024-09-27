using System.Linq.Expressions;
using domain.Entities;
using domain.Models.Request.Task;
using domain.Models.Response.Task;
using domain.Models.Response.TaskComments;
using infra.Data.Repository;
using TaskAttachmentInfos = domain.Models.Response.TaskAttachment.TaskAttachmentInfos;

namespace service.Services;

public class ProjectTaskService
{
    private readonly ProjectTaskRepository _projectTaskRepository;
    private readonly BaseRepository<ProjectTask> _projectTaskRepositoryBase;
    private readonly BaseRepository<TaskAttachment> _taskAttachmentRepositoryBase;
    private readonly BaseRepository<TaskComment> _taskCommentRepositoryBase;

    public ProjectTaskService(BaseRepository<ProjectTask> projectTaskRepositoryBase, BaseRepository<TaskAttachment> taskAttachmentRepositoryBase,
        ProjectTaskRepository projectTaskRepository, BaseRepository<TaskComment> taskCommentRepositoryBase)
    {
        _projectTaskRepositoryBase = projectTaskRepositoryBase;
        _taskCommentRepositoryBase = taskCommentRepositoryBase;
        _taskAttachmentRepositoryBase = taskAttachmentRepositoryBase;
        _projectTaskRepository = projectTaskRepository;
    }

    public async Task<ServiceResponse<string>> CreateProjectTask(int userId, int projectId, CreateTaskModel body)
    {
        try
        {
            var serviceResponse = new ServiceResponse<string>();


            var projectTask = new ProjectTask
            {
                Title = body.Title,
                Description = body.Description,
                ProjectId = projectId,
                Deadline = DateTime.Parse(body.Deadline).ToUniversalTime()
            };

            await _projectTaskRepositoryBase.Add(projectTask);

            foreach (var attachmentInfos in body.Attachments)
            {
                var attachment = new TaskAttachment()
                {
                    UserId = userId,
                    ProjectTaskId = projectTask.Id,
                    Name = attachmentInfos.Name,
                    Attachment = attachmentInfos.Attachment,
                    Created = DateTime.UtcNow,
                };

                await _taskAttachmentRepositoryBase.Add(attachment);
            }

            serviceResponse.Status = 200;
            serviceResponse.Message = "Tarefa criada!";
            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public async Task<ServiceResponse<TaskInfos>> GetTask(int projectTaskId)
    {
        try
        {
            var serviceResponse = new ServiceResponse<TaskInfos>();
            var task = await _projectTaskRepository.GetTask(projectTaskId);

            var taskInfos = new TaskInfos
            {
                Id = task.Id,
                Description = task.Description,
                Title = task.Title,
                Deadline = task.Deadline,
                Completed = task.Completed,
                TaskAttachments = task.TaskAttachments.Select(x => new TaskAttachmentInfos
                {
                    Id = x.Id,
                    Name = x.Name,
                    Attachment = x.Attachment,
                    ProjectTaskId = x.ProjectTaskId,
                    UserId = x.UserId,
                    UserName = x.User.Name,
                    UserType = x.User.TypeUser,
                    Created = x.Created
                }),

                TaskComments = task.TaskComments.Select(x => new TaskCommentInfo
                {
                    Id = x.Id,
                    Comment = x.Comment,
                    UserId = x.UserId,
                    UserName = x.User.Name,
                    UserType = x.User.TypeUser,
                    Created = x.Created
                })
            };


            serviceResponse.Status = 200;
            serviceResponse.Message = "OK!";
            serviceResponse.Data = taskInfos;

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<string>> AddAttachment(int projectTaskId, AddAttachmentModel body)
    {
        try
        {
            var serviceResponse = new ServiceResponse<string>();
            await _taskAttachmentRepositoryBase.Add(new TaskAttachment
            {
                Name = body.Name,
                Attachment = body.Attachment,
                ProjectTaskId = projectTaskId,
                UserId = body.UserId,
                Created = DateTime.UtcNow,

            });

            serviceResponse.Status = 200;
            serviceResponse.Message = "Anexo adicionado!";
            serviceResponse.Data = null;

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<string>> AddComment(int projectTaskId, AddCommentModel body)
    {
        try
        {
            var serviceResponse = new ServiceResponse<string>();
            await _taskCommentRepositoryBase.Add(new TaskComment
            {
                UserId = body.UserId,
                TaskId = projectTaskId,
                Comment = body.Comment,
                Created = DateTime.UtcNow,
            });


            serviceResponse.Status = 200;
            serviceResponse.Message = "Comentário criado!";
            serviceResponse.Data = null;

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<string>> CompleteTask(int taskId)
    {
        try
        {
            var serviceResponse = new ServiceResponse<string>();
            var task = await _projectTaskRepositoryBase.GetById(taskId);

            task.Completed = true;
            await _projectTaskRepositoryBase.Update(task);


            serviceResponse.Status = 200;
            serviceResponse.Message = "Task completa!";
            serviceResponse.Data = null;

            return serviceResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
