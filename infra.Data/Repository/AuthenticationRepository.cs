using domain.Entities;
using infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace infra.Data.Repository;

public class AuthenticationRepository
{
    protected readonly NpSqlContext _npSqlContext;

    public AuthenticationRepository(NpSqlContext npSqlContext)
    {
        _npSqlContext = npSqlContext;
    }

    public async Task<Authentication?> GetByUserId(int userId)
    {
        return await _npSqlContext.Set<Authentication>().FirstOrDefaultAsync(x => x.UserId.Equals(userId));
    }
}
