using domain.Entities;
using infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace infra.Data.Repository;

public class ProjectTaskRepository
{
    protected readonly NpSqlContext _npSqlContext;

    public ProjectTaskRepository(NpSqlContext npSqlContext)
    {
        _npSqlContext = npSqlContext;
    }

    public async Task<ProjectTask> GetTask(int taskId)
    {
        return await _npSqlContext.Set<ProjectTask>()
            .Where(x => x.Id == taskId)
            .Include(x => x.TaskAttachments)
            .ThenInclude(x => x.User)
            .Include(x => x.TaskComments)
            .ThenInclude(x => x.User)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
}
