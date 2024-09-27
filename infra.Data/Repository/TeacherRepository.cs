using domain.Entities;
using domain.Enums;
using infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace infra.Data.Repository;

public class TeacherRepository
{
    protected readonly NpSqlContext _npSqlContext;

    public TeacherRepository(NpSqlContext npSqlContext)
    {
        _npSqlContext = npSqlContext;
    }

    public async Task<ICollection<Teacher>> GetAllAvailableTeachersForOrientantion()
    {
        return await _npSqlContext.Set<Teacher>()
            .Include(x => x.User)
            .Where(x => x.AvailableOrientation)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<Teacher>> GetAllAvailableTeachersForEvaluation()
    {
        return await _npSqlContext.Set<Teacher>()
            .Include(x => x.User)
            .Where(x => x.AvailableEvaluation)
            .AsNoTracking()
            .ToListAsync();
    }
}
