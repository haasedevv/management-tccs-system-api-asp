using domain.Entities;
using domain.Enums;
using infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace infra.Data.Repository;

public class UserRepository
{
    protected readonly NpSqlContext _npSqlContext;

    public UserRepository(NpSqlContext npSqlContext)
    {
        _npSqlContext = npSqlContext;
    }

    public async Task<User?> GetByLogin(string login)
    {
        return await _npSqlContext.Set<User>().FirstOrDefaultAsync(x => x.Login.Equals(login));
    }

    public async Task<IEnumerable<User>> GetStudentsGroup(int projectId)
    {
        return await _npSqlContext.Set<User>()
                .Where(x => x.TypeUser == TypeUserEnum.Student && x.Requests.Any(requestItem => requestItem.ProjectId == projectId))
                .Include(x => x.Requests).AsNoTracking().ToListAsync();
    }
}
