using domain.Entities;
using domain.Enums;
using infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace infra.Data.Repository;

public class RequestRepository
{
    protected readonly NpSqlContext _npSqlContext;

    public RequestRepository(NpSqlContext npSqlContext)
    {
        _npSqlContext = npSqlContext;
    }

    public async Task<ICollection<User>> GetProjectStudents(int projectId)
    {
        return await _npSqlContext.Set<Request>()
            .Where(x => x.ProjectId == projectId && x.User.TypeUser == TypeUserEnum.Student)
            .Select(x => x.User)
            .AsNoTracking()
            .ToListAsync();
    }
}
