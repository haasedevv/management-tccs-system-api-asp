using domain.Entities;
using domain.Enums;
using infra.Data.Context;
using Microsoft.EntityFrameworkCore;
namespace infra.Data.Repository;

public class StudentRepository
{
    protected readonly NpSqlContext _npSqlContext;

    public StudentRepository(NpSqlContext npSqlContext)
    {
        _npSqlContext = npSqlContext;
    }

    public async Task<ICollection<User>> GetAllAvailableStudentsForOrientantion()
    {
        return await _npSqlContext.Set<User>()
            .Where(x => x.Available && x.TypeUser == TypeUserEnum.Student)
            .AsNoTracking()
            .ToListAsync();
    }
}




